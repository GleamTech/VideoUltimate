# ![VideoUltimate Logo](videoultimate-logo.png) VideoUltimate: .NET Video Reader and Thumbnailer
VideoUltimate is the fastest and easiest .NET Video Reader and Thumbnailer library for reading video files without any dependencies.

- Read any video file format on the planet.

- Read a video file frame by frame.

- Generate meaningful thumbnails.

- Read info and metadata of the video file.

![.NET Video Reader and Thumbnailer](videoultimate.png)

**Note:** This project contains a fully working version of the product, however without a license key it will run in trial mode. For more information, please see [VideoUltimate: .NET Video Reader and Thumbnailer](http://www.gleamtech.com/videoultimate) product page.

### To use VideoUltimate in a project, do the following in Visual Studio:

1.  Add references to VideoUltimate assemblies. There are two ways to perform this:

    -   Add reference to **GleamTech.Core.dll** and **GleamTech.VideoUltimate.dll** found in "Bin" folder of VideoUltimate-vX.X.X.X.zip package which you already downloaded and extracted.

    -   Or install NuGet package and add references automatically via NuGet Package Manager in Visual Studio: 
        Go to **Tools -> NuGet Package Manager -> Package Manager Console** and run this command:

			`Install-Package VideoUltimate -Source https://get.gleamtech.com/nuget/default/`

		If you prefer using the user interface when working with NuGet, you can also install the package this way:
		
			-  GleamTech has its own NuGet feed so first you need to add this feed to be able to find GleamTech's packages. 
			    Go to **Tools -> NuGet Package Manager -> Package Manager Settings** and then click the **+** button to add a 
			    new package source. Enter `GleamTech` in **Name** field and `https://get.gleamtech.com/nuget/default/` 
			    in **Source** field and click **OK**.
			    
			-  Go to **Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution**, select `GleamTech` or `All` 
			   in the Package source dropdown on the top right. Now enter `VideoUltimate` in the search field, 
			   and click **Install** button on the found package.

2.  Set VideoUltimate's global configuration. For example, you may want to set the license key. Insert the following line into the ```Application_Start``` method of your **Global.asax.cs** for Web projects or Main method for other project types:

    ```
    //Set this property only if you have a valid license key, otherwise do not
    //set it so VideoUltimate runs in trial mode.
    VideoUltimateConfiguration.Current.LicenseKey = "QQJDJLJP34...";
    ```

    Alternatively you can specify the configuration in ```<appSettings>``` tag of your Web.config.

    ```
    <appSettings>
      <add key="VideoUltimate:LicenseKey" value="QQJDJLJP34..." />
    </appSettings>
    ```

    As you would notice, ```VideoUltimate:``` prefix maps to ```VideoUltimateConfiguration.Current```.

3.  Open one of your class files (eg. Program.cs) and at the top of your file add ```GleamTech.VideoUltimate``` namespace:

    ```
    using GleamTech.VideoUltimate;
    ```

    Now in some method insert these lines:

    ```
    using (var videoFrameReader = new VideoFrameReader(@"C:\Video.mp4"))
    {
        if (videoFrameReader.Read())
        {
            using (var frame = videoFrameReader.GetFrame())
                frame.Save(@"C:\Frame1.jpg", ImageFormat.Jpeg);
        }
    }
    ```

    This will open the source video "C:\Video.mp4", read the first frame, and if the frame is read and decoded successfully, it will get a Bitmap instance of the frame and save it as "C:\Frame1.jpg".

    Sometimes you may only need to quickly generate a meaningful thumbnail for a video, you can use VideoThumbnailer class for this:

    ```
    using (var videoThumbnailer = new VideoThumbnailer(@"C:\Video.mp4"))
    using (var thumbnail = videoThumbnailer.GenerateThumbnail(100))
        thumbnail.Save(@"C:\Thumbnail1.jpg", ImageFormat.Jpeg);    
    ```
