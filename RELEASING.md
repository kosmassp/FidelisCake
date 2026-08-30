# Releasing FidelisCake

How an update reaches a shop:

1. The till reads [`version.txt`](version.txt) from this repository
   (`https://raw.githubusercontent.com/kosmassp/FidelisCake/master/version.txt`).
2. If `Version:` is newer than what it is running, it offers the update to the operator.
3. On yes, it downloads the `File:` ZIP from the GitHub release, unpacks it, and restarts
   through `UpdateInstaller`, which backs up every file it overwrites into
   `Backup\<timestamp>` beside the exe. **The installer never deletes anything** — files
   absent from the ZIP are simply left as they are.

That last rule is why the ZIP contents below are an explicit list: whatever is in the ZIP
lands on every till, and whatever is not stays untouched.

> **Transition note.** Tills deployed before the updater existed (1.0.1.3 and older) get their
> first GitHub-era build by hand: copy the ZIP's contents over the installation, keeping the
> till's own `InventoryAndSales.exe.config`. From then on they update themselves — the manifest
> address is built into the application, and an installation that was seeded with the old Google
> Doc address rewrites it to `version.txt` on its first start
> (`DBUtility.RetireSupersededManifestUrl`). The Doc only needs to stay correct while some
> machine still runs a Doc-era build (step 7).

---

## One-time setup (before the first GitHub release)

- [ ] Backfill the current version so `version.txt` points at something real: create a
      release on <https://github.com/kosmassp/FidelisCake/releases>, tag `v1.0.1.3`,
      and upload the existing v1.0.1.3 ZIP from `InventoryAndSales\bin\Release`
      **renamed to `InventoryAndSales.zip`**. Upload that file byte-for-byte as it is:
      `version.txt` already carries its SHA-256
      (`5da79f82767cac8b6d546d89915097747fcc787a417a9104c27d0ca6bb8095b1`), and a
      re-zipped copy would hash differently.
- [ ] After `version.txt` reaches `master`, open
      <https://raw.githubusercontent.com/kosmassp/FidelisCake/master/version.txt> in a
      browser and check it renders as plain text starting with the `#` comment block.

---

## Every release

With the Actions workflow ([.github/workflows/release.yml](.github/workflows/release.yml)),
steps 2, 3 and 5 run by themselves. Bump the version (step 1), get it onto `master`, then:

```powershell
git tag v1.0.1.4
git push origin v1.0.1.4
```

The release appears with the ZIP attached and its SHA-256 in the body. Continue at step 4
(smoke-test the ZIP you just released), then step 6 — the release body contains the
`version.txt` block ready to paste. The workflow refuses a tag that does not match
`AssemblyVersion`, so a forgotten bump fails loudly instead of releasing a build that
reports the wrong version. The steps below double as the manual fallback.

### 1. Bump the version

Edit `InventoryAndSales/Properties/AssemblyInfo.cs` — **both** lines:

```
[assembly: AssemblyVersion("1.0.1.4")]
[assembly: AssemblyFileVersion("1.0.1.4")]
```

The update offer compares this number and nothing else. A release without a bump is
invisible to every till.

### 2. Build

Use the VS2022 MSBuild — the old Framework one in `C:\WINDOWS\Microsoft.NET\...` fails on
C# 6 syntax with `CS1056`, which means wrong compiler, not broken code:

```
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" InventoryAndSalesProject.sln /t:Rebuild /p:Configuration=Release
```

### 3. Assemble `InventoryAndSales.zip`

The csproj curates the Release output: alongside the application it copies in the database
providers (`Npgsql.dll` with its `System.*` helpers, `System.Data.SQLite.dll`), the native
SQLite engines (which must keep their `x64\` and `x86\` sub-folders), and the two `Report\`
assets. The package is therefore simply **the build output, minus what must never reach a
till**:

- **`InventoryAndSales.exe.config`** — it holds each shop's `DatabaseProvider` and
  `ConnectionString`. Shipping it would overwrite a shop's database settings on update.
  Left out of the ZIP, the installed config survives untouched, because the installer
  never deletes anything. If a release truly needs a config change, say so in the release
  notes and handle it shop by shop.
- **`SalesInventory.db`** or any `*.db` — a test database created by running the exe from
  `bin\Release` would silently replace a shop's live data file. Check for this every time.
- `Log\`, `Backup\`, and old release ZIPs sitting in `bin\Release`.

`InventoryAndSales.exe` must sit at the ZIP root (one wrapper folder is also accepted;
deeper is refused by the staging check), and the ZIP must stay under 200 MB — the till
refuses bigger. From `InventoryAndSales\bin\Release`, in PowerShell:

```powershell
robocopy . pkg /S /XD Log Backup pkg /XF InventoryAndSales.exe.config *.zip *.db
Compress-Archive -Path pkg\* -DestinationPath InventoryAndSales.zip -Force
Remove-Item -Recurse -Force pkg
```

### 4. Smoke-test the ZIP

Extract it to an empty folder, drop in a copy of a known-good `InventoryAndSales.exe.config`,
run the exe, log in, and use **Periksa Pembaruan** — it must answer
*"Aplikasi sudah versi terbaru (1.0.1.4)"* with the new number. Open one report and check
it is sortable/searchable (proves the `Report\` assets made it in).

### 5. Publish the GitHub release

Pushing a `v1.0.1.4` tag does this for you, together with steps 2 and 3. By hand instead:
on <https://github.com/kosmassp/FidelisCake/releases> → *Draft a new release*:

- Tag: `v1.0.1.4` (the `v` prefix, on `master`)
- Title: `1.0.1.4`
- Description: the release notes, any length
- Attach `InventoryAndSales.zip` — the name must stay exactly that; it is part of the URL

### 6. Update `version.txt` — only after the release is live

This is the step that actually rolls the update out; nothing before it reaches any till.
The workflow puts this exact block, hash filled in, in the release body — copy it, write
the `Notes` line, done:

```
Version: 1.0.1.4
Drive:   https://github.com/kosmassp/FidelisCake/releases
File:    https://github.com/kosmassp/FidelisCake/releases/download/v1.0.1.4/InventoryAndSales.zip
Sha256:  <SHA-256 of the ZIP - releasing manually, compute it with: Get-FileHash InventoryAndSales.zip>
Notes:   Satu baris, bahasa Indonesia - ini yang dibaca operator di kasir.
```

A till checks the downloaded archive against `Sha256` and refuses a mismatch, so the hash
must be of the exact file attached to the release — never re-zip after computing it.

Commit to `master`, push, and open the raw URL to confirm. GitHub's raw CDN caches for
about five minutes, so a just-pushed change can take that long to appear.

### 7. Mirror to the Google Doc (only while Doc-era builds remain)

A machine that was seeded with the Doc address and has not yet taken a GitHub-era build still
reads the Doc. Edit it so its four lines match `version.txt` exactly. Once every such machine
has updated once, this step disappears — the migration rewrites their stored address.

### 8. Verify end-to-end

On a machine or spare copy running the **previous** version: **Periksa Pembaruan** →
the offer for the new version appears → accept → the app closes, updates and restarts →
**Periksa Pembaruan** now reports the new version. Confirm a `Backup\<timestamp>` folder
appeared beside the exe.

---

## If a release goes wrong

- **Before tills have taken it:** point `version.txt` (and the Doc) back at the previous
  version's lines. Tills that never updated will simply see nothing new.
- **After a till has updated:** its previous files are in `Backup\<timestamp>` beside the
  exe — copy them back over the installation. Release assets for old versions stay
  downloadable from the releases page, so a manual re-install of any version is always
  possible.
- A till that fails mid-update restores its own backup and restarts on the old version by
  itself; check `Log\log.txt` on that machine before retrying.
