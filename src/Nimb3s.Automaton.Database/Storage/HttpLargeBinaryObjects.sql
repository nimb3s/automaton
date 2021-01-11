ALTER DATABASE [$(DatabaseName)]
    ADD FILEGROUP [HttpLargeBinaryObjects];
GO
ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [HttpLargeBinaryObjects00],
		FILENAME = '$(DataPath)$(DatabaseName)\HttpLargeBinaryObjects00.ndf', SIZE = 100MB, FILEGROWTH = 100MB
	)
	TO FILEGROUP [HttpLargeBinaryObjects];
GO

