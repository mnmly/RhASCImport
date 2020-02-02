## ASC Import Plugin for Rhino

### Installation
- Download [RhASCImport.rhp](https://github.com/mnmly/RhASCImport/releases)
- [Mac] Place `RhASCImport.rhp` into `~/Library/Application Support/McNeel/Rhinoceros/MacPlugIns/`
- [Windows] Compress `RhASCImport.rhp` into zip (`RhASCImport.zip`). Then change the file extension to `.rhi` (`RhASCImport.rhi`). Double click the file, and Rhino should launch the installation wizard.

### Usage
`File` → `Import…` → `Options` → `Esri ASCII raster format (asc)` → `Open`
![asc-import-option](https://user-images.githubusercontent.com/317202/73608931-7825c500-45c0-11ea-957f-92b80c9750b8.png)

### In Action
![asc-import](https://user-images.githubusercontent.com/317202/73608898-1feec300-45c0-11ea-90b3-03ca66d9c980.png)


### Note
It also generates PointCloud object. Make sure you either hide or delete the point cloud object if you don't use it.
