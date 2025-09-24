# AppSettings Implementation Guide

## Overview
The AppSettings system is implemented following SOLID principles, IoC (Inversion of Control), and MVVM patterns. Here's how to use it:

## Architecture

### 1. **IAppSettingsService Interface**
- Provides async methods for getting/setting settings with type safety
- Thread-safe operations with caching
- Automatic save/load functionality

### 2. **AppSettingsService Implementation**
- Uses `ConcurrentDictionary` for thread-safe caching
- `SemaphoreSlim` for file access synchronization
- JSON serialization for persistence
- Error handling and validation

### 3. **AppSettingsModel**
- Implements `INotifyPropertyChanged` for UI binding
- Strongly-typed properties for settings
- Async load/save operations

### 4. **AppSettingsViewModel**
- MVVM pattern implementation
- Commands for Save/Load/Reset operations
- Status messages and loading states

## Usage Examples

### Getting a Setting
```csharp
var connectionString = await _appSettingsService.GetSettingAsync<string>("IotHubConnectionString", "");
var interval = await _appSettingsService.GetSettingAsync<int>("RefreshIntervalSeconds", 60);
var enabled = await _appSettingsService.GetSettingAsync<bool>("EnableNotifications", true);
```

### Setting a Value
```csharp
await _appSettingsService.SetSettingAsync("IotHubConnectionString", "your-connection-string");
await _appSettingsService.SetSettingAsync("RefreshIntervalSeconds", 30);
await _appSettingsService.SaveAsync(); // Persist to file
```

### Using the Model
```csharp
// Load all settings
await _appSettingsModel.LoadSettingsAsync();

// Access properties
var connectionString = _appSettingsModel.IotHubConnectionString;
_appSettingsModel.RefreshIntervalSeconds = 30;

// Save changes
await _appSettingsModel.SaveSettingsAsync();
```

### XAML Binding
```xml
<TextBox Text="{Binding AppSettings.IotHubConnectionString}" />
<CheckBox IsChecked="{Binding AppSettings.EnableNotifications}" />
<Button Content="Save" Command="{Binding SaveCommand}" />
```

## Dependency Injection Setup

The system is configured through `ServiceConfiguration.cs`:

```csharp
services.AddSingleton<IAppSettingsService>(provider => 
    new AppSettingsService("config.json"));
services.AddSingleton<AppSettingsModel>();
services.AddTransient<AppSettingsViewModel>();
```

## Benefits

1. **Thread-Safe**: All operations are thread-safe with proper locking
2. **Type-Safe**: Generic methods provide compile-time type checking
3. **Async**: All I/O operations are asynchronous
4. **Cached**: Settings are cached in memory for performance
5. **Testable**: Interface-based design allows easy mocking
6. **MVVM Ready**: Full support for data binding and commands
7. **Error Handling**: Comprehensive error handling with meaningful exceptions
8. **Auto-Save**: Changes are tracked and can be persisted automatically

## Best Practices

1. Always use the async methods
2. Handle exceptions appropriately
3. Use dependency injection for service access  
4. Bind to ViewModels in XAML, not directly to services
5. Initialize settings early in application lifecycle
6. Use strongly-typed properties in your models