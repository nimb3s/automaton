ALTER DATABASE [$(DatabaseName)]
    ADD FILEGROUP [HttpIndex];
GO

ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [HttpIndex00],
		FILENAME = '$(IndexPath)$(DatabaseName)\HttpIndex00.ndf', SIZE = 100MB, FILEGROWTH = 100MB
	)
	TO FILEGROUP [HttpIndex];
GO
	