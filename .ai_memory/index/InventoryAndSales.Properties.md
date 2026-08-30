# Directory: `InventoryAndSales/Properties/`

Namespace `InventoryAndSales.Properties`. Visual Studio generated project metadata. **Do not
hand-edit the `.Designer.cs` files** — edit the `.resx` / `.settings` source through the IDE and let
them regenerate.

| File | Purpose |
|---|---|
| `AssemblyInfo.cs` | Assembly attributes: title, description, company, product, copyright, GUID, `AssemblyVersion` / `AssemblyFileVersion`. **`MainForm` reads this version and appends it to the window title** (`Assembly.GetEntryAssembly().GetName().Version`), so bumping it here changes what the user sees. |
| `Resources.resx` | Embedded resource definitions (icons, images). |
| `Resources.Designer.cs` | Generated strongly-typed accessors — `Resources.ResourceManager`, `Resources.Culture`, one property per resource. |
| `Settings.settings` | Application settings definition. Empty in practice. |
| `Settings.Designer.cs` | Generated `Settings.Default` accessor. |

⚠ Note: runtime configuration does **not** go through `Settings.Default`. The connection string and
printer name are read with `ConfigurationManager.AppSettings[...]` from `App.config` — see
`Database/DBFactory.cs` and `SimpleCommon/Utility/PrinterUtility.cs`.
