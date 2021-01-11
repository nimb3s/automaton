ALTER DATABASE [$(DatabaseName)]
    ADD FILEGROUP [HttpData];
GO


ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [ServicesData00],
		FILENAME = '$(DataPath)$(DatabaseName)\HttpData00.ndf', SIZE = 100MB, FILEGROWTH = 100MB
	)
	TO FILEGROUP [HttpData];
GO
	