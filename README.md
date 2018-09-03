# DownloadRouter
📦 Simple download router to move files after download completes

# Configuration
On Configuration.json file
```
{
  "TeraCopyPath": "C:\\Program Files\\TeraCopy\\TeraCopy.exe", //Path to TeraCopy, leave empty if not using TeraCopy

  "FilesFilter": [ //File extensions to move after download completes, use *.* to move every file
    "*.mp4",
    "*.mkv",
    "*.avi"
  ],

  "SourcesDirectories": [ //Source directories to use, not specified will be ignored
    "D:\\Downloads1",
    "D:\\Downloads2"
  ],

  "DestinationMappings": [ //Define path filters and destinations for each filter
    {
      "NameFilter": "Filter1",
      "DestinationDirectories": [
        "D:\\Destination1",
        "D:\\Destination2"
      ]
    },

    {
      "NameFilter": "Filter2",
      "DestinationDirectories": [
        "D:\\Destination2"
      ]
    }
  ]
}
```

# Usage:
`dotnet DownloadRouter.dll -p PathToDir`
