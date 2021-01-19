ALTER DATABASE [$(DatabaseName)]
    ADD FILEGROUP [JobData];
GO


ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [JobData00],
		FILENAME = '$(DataPath)$(DatabaseName)\JobData00.ndf', SIZE = 100MB, FILEGROWTH = 100MB
	)
	TO FILEGROUP [JobData];
GO
	