# Terraria World Host Transfer

A cross-platform desktop app that simplifies hosting Terraria worlds with friends via **direct peer-to-peer transfer** — no Discord file uploads, no manual file management.

Built with **.NET 10** and **Avalonia UI** for a modern, native experience on Windows, Linux, and macOS.

---

## Features

- 🗺️ **Automatic world detection** from tModLoader
- 🚀 **One-click host transfer** via TCP (no third-party services)
- 📦 **Automatic archiving** with transfer history
- 🔄 **Restore disabled worlds** easily
- 📊 **Real-time progress tracking** during transfers
- 🎨 **Modern dark UI** with smooth navigation

---

## Why?

Playing Terraria with friends using using native Host&Play like with "Play via Steam" requires the **original host to always be online**. This app lets you:

1. Transfer world hosting to another player
2. Archive the previous version automatically
3. Continue playing without the original host

No more Discord file limits, no manual zip management, no confusion.

---

## How It Works

### **Sending a world (Host):**
1. Select your world from the list
2. Click "Start Transfer" — app listens on port 7777
3. Share your IP with the recipient
4. Transfer happens automatically
5. Your world is archived and disabled locally

### **Receiving a world (Client):**
1. Click "Receive World"
2. Enter the host's IP address
3. World is downloaded, extracted, and ready to play

That's it. Clean, simple, direct.

---

## Project Structure
```text
TerrariaWorldSwitcher/
├── Core/                    # Business logic
│   ├── WorldScanner.cs      # Detects worlds in tModLoader
│   ├── WorldStager.cs       # Prepares worlds for transfer
│   ├── WorldZipper.cs       # Creates archives
│   ├── WorldArchiver.cs     # Manages version history
│   ├── WorldDisabler.cs     # Moves worlds out of active folder
│   └── WorldRestorer.cs     # Restores archived/disabled worlds
│
├── /Core/WorldTransfer/     # Network & transfer
│   ├── WorldSender.cs       # TCP server for sending
│   ├── WorldReceiver.cs     # TCP client for receiving
│   └── TransferProtocol.cs  # Shared network constants
│
├── Models/
│   ├── WorldInfo.cs         # World metadata
│   ├── AppState.cs          # Application states
│   └── Paths.cs             # File system paths
│
└── UI/                      # Avalonia desktop app
    ├── Assets/              #Logos
    ├── Views/               # Page components
    │   ├── *
    └── Models/
        └── WorldDisplayInfo # UI-specific world data
        
```

---

## Technical Details

- **Framework:** .NET 10
- **UI:** Avalonia 11.3 (cross-platform native UI)
- **Transfer:** Raw TCP sockets
- **Compression:** Built-in .NET `ZipFile`
- **Storage:** Local file system (no database)

---

## Planned Features

- [ ] Steam integration for friend discovery and other utils
- [ ] World history viewer with restore from any version
- [ ] QR code for easy IP sharing
- [ ] Auto-detect local network IPs
- [ ] Port forwarding helper for internet transfers

---

## Disclaimer

This project is a fan tool for Terraria/tModLoader.  
Not affiliated with Re-Logic or tModLoader team.

World files remain your property. Transfer at your own risk.

---

## License

**Dual Licensed:**

### Non-Commercial Use (Free)
This project is free to use for:
- ✅ Personal use
- ✅ Educational purposes
- ✅ Open source projects
- ✅ Non-profit organizations

**Licensed under CC BY-NC 4.0** — you must credit the author and cannot use it commercially.

### Commercial Use (Contact Required)
Want to use this in a:
- 💰 Paid product or service?
- 💰 Monetized platform (ads, subscriptions)?
- 💰 For-profit company?

**You need permission.** Contact me via:
- **GitHub Issues:** [Open a discussion](https://github.com/jolojo500/TerrariaWorldSwitcher/issues)
- **Discord:** [ton username]

I'm reasonable and open to collaboration — just don't use my work to make money without talking to me first. 🤝

---

**Bottom line:** Free for everyone except people trying to profit from my work without contributing back.