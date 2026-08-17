# Directory: `SimpleCommon/Properties/`

Namespace `SimpleCommon.Properties`. Visual Studio generated library metadata. **Do not hand-edit
the `.Designer.cs` files** — edit the `.resx` through the IDE and let it regenerate.

| File | Purpose |
|---|---|
| `AssemblyInfo.cs` | Assembly attributes for the `SimpleCommon` library: title, description, company, product, copyright, GUID, `AssemblyVersion` / `AssemblyFileVersion`. Unlike the application's copy, nothing reads this version at runtime. |
| `Resources.resx` | Embedded resource definitions. |
| `Resources.Designer.cs` | Generated strongly-typed accessors — `Resources.ResourceManager`, `Resources.Culture`, one property per resource. |

There is no `Settings.settings` here. `PrinterUtility` reads `PrinterName` from the **hosting
application's** `App.config` via `ConfigurationManager.AppSettings`, which is why the library
references `System.configuration`.

⚠ Note: that makes the library implicitly dependent on a config key the host must define — a hidden
coupling. Passing the printer name into `PrinterUtility.Print` would make the dependency explicit
and keep the library self-contained.
