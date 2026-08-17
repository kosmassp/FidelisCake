# Directory: `SimpleCommon/Properties/`

Namespace `SimpleCommon.Properties`. Visual Studio generated library metadata. **Do not hand-edit
the `.Designer.cs` files** — edit the `.resx` through the IDE and let it regenerate.

| File | Purpose |
|---|---|
| `AssemblyInfo.cs` | Assembly attributes for the `SimpleCommon` library: title, description, company, product, copyright, GUID, `AssemblyVersion` / `AssemblyFileVersion`. Unlike the application's copy, nothing reads this version at runtime. |
| `Resources.resx` | Embedded resource definitions. |
| `Resources.Designer.cs` | Generated strongly-typed accessors — `Resources.ResourceManager`, `Resources.Culture`, one property per resource. |

There is no `Settings.settings` here, and the library reads no configuration of its own.

`PrinterUtility` used to reach into the **hosting application's** `App.config` for `PrinterName` — a
hidden coupling to a key the host had to define. It now takes a `PrintSettings` argument, so the
dependency is explicit and the library is self-contained.
