ALTER DATABASE [$(DatabaseName)]
    ADD FILEGROUP [JobIndex];
GO

ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [JobIndex00],
		FILENAME = '$(IndexPath)$(DatabaseName)\JobIndex00.ndf', SIZE = 100MB, FILEGROWTH = 100MB
	)
	TO FILEGROUP [JobIndex];
GO
	