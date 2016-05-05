# Settings
Small library for deserialize a dictionary containing hierarchy based settings into a matching object model.

## Why?

Ever worked in a project were system wide settings was stored in a table in a database and for each new setting you have to add a new column?
Wouldn't it be nice to change this structure to use a key-value based style and also get the ability to deserialize the data to a matching object model?
This is actually what you can get by using this small library.

## How it works

Lets say you have a table called `Setting` in your database containing two columns `key` and `value`. By fetching this data as a dictionary
you can then use `SettingDeserializer` to map the settings to a matching model. No type information is needed in the database instead the deserialization
process will try to convert the value to match the property in model.

By convention it assumes that the structure of the settings is hierarchy based were each level is separated by a dot (.). When a setting is in the top of 
the hierarchy, e.g `IsActive`, the deserializer will assume that a matching property will be found in the top of the object model.

**Sample settings**

```
Dictionary<string, string> settings;
settings.Add("Http.Port", "80");
settings.Add("IsActive", "true");
settings.Add("DefaultPage", "index.html");
```

**Object model**

```
class WebServerSettings
{
  public string DefaultPage { get; set; }
  public bool IsActive { get; set; }
  public Http Http { get; set; }
}

class Http
{
  public int Port { get; set; }
}
```

**Deserialization to object model**

```
SettingDeserializer deserializer = new SettingDeserializer();
WebServerSettings webServerSettings = deserializer.Deserialize<WebServerSettings>(settings);
```
