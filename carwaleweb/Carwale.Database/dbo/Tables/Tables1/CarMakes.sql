CREATE TABLE [dbo].[CarMakes] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]            VARCHAR (30)  NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_CarMakes_IsActive] DEFAULT (0) NOT NULL,
    [LogoUrl]         VARCHAR (50)  CONSTRAINT [DF_CarMakes_LogoUrl] DEFAULT ('nologo.gif') NOT NULL,
    [Used]            BIT           CONSTRAINT [DF_CarMakes_Used] DEFAULT (1) NOT NULL,
    [New]             BIT           CONSTRAINT [DF_CarMakes_New] DEFAULT (1) NOT NULL,
    [Indian]          BIT           CONSTRAINT [DF_CarMakes_Indian] DEFAULT (1) NOT NULL,
    [Imported]        BIT           CONSTRAINT [DF_CarMakes_Imported] DEFAULT (0) NOT NULL,
    [Futuristic]      BIT           CONSTRAINT [DF_CarMakes_Futuristic] DEFAULT (0) NOT NULL,
    [Classic]         BIT           CONSTRAINT [DF_CarMakes_Classic] DEFAULT (0) NOT NULL,
    [Modified]        BIT           CONSTRAINT [DF_CarMakes_Modified] DEFAULT (0) NOT NULL,
    [MaCreatedOn]     DATETIME      NULL,
    [MaUpdatedBy]     NUMERIC (18)  NULL,
    [MaUpdatedOn]     DATETIME      NULL,
    [IsReplicated]    BIT           CONSTRAINT [DF__CarMakes__IsRepl__6A214711] DEFAULT ((0)) NULL,
    [HostURL]         VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    [PriorityOrder]   INT           NULL,
    [OriginalImgPath] VARCHAR (150) NULL,
    CONSTRAINT [PK_CarMakes] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [IX_CarMakes] UNIQUE NONCLUSTERED ([Name] ASC) WITH (FILLFACTOR = 90)
);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in CarMakes Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCarMakesLogs]
   ON [dbo].[CarMakes]
   FOR INSERT,UPDATE
AS 
DECLARE @NoOfRows INT
DECLARE  @NextRowId INT
DECLARE  @LoopIndex INT
DECLARE   @CurRowId INT
BEGIN
SET @NoOfRows = @@ROWCOUNT
	
  IF @NoOfRows = 1
	BEGIN
		IF ((SELECT COUNT(*) FROM CarMakes AS CM WITH (NOLOCK)
						JOIN deleted AS I ON CM.ID=I.ID)=0)
			 BEGIN 
		 
		 INSERT INTO  CarWaleMasterDataLogs
											(TableName,
											 AffectedId,
											 Remarks,
											 ColumnName,
											 OldValue,
											 NewValue,
											 CreatedOn
											 )
		  SELECT 'CarMakes',
				  ID,
				  'Record Inserted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM inserted										 
		  
		 END               
	ELSE 
	   BEGIN 
				   
				   IF (UPDATE(Name))
			BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Name',
							  D.Name,
							  I.Name,
							  GETDATE()
							FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END 
			IF (UPDATE(IsDeleted))
			BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'IsDeleted',
							  D.IsDeleted,
							  I.IsDeleted,
							  GETDATE()
						 FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END 
			
	
        IF (UPDATE(LogoUrl))
		BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'LogoUrl',
							  D.LogoUrl,
							  I.LogoUrl,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END 
	
      IF (UPDATE(Used))
		BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Used',
							  D.Used,
							  I.Used,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END 
     IF (UPDATE(New))
	BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'New',
							  D.New,
							  I.New,
							  GETDATE()
							FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END
      IF (UPDATE(Indian))
	BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Indian',
							  D.Indian,
							  I.Indian,
							  GETDATE()
						 FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END
			IF (UPDATE(Imported))
			BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Imported',
							  D.Imported,
							  I.Imported,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END 
			IF (UPDATE(Futuristic))
			BEGIN
			 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														)
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Futuristic',
							  D.Futuristic,
							  I.Futuristic,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END 
			IF (UPDATE(Classic))
			BEGIN
			 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Classic',
							  D.Classic,
							  I.Classic,
							  GETDATE()
					     FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END
			IF (UPDATE(IsReplicated))
			BEGIN
			 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'IsReplicated',
							  D.IsReplicated,
							  I.IsReplicated,
							  GETDATE()
					     FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			END 
			IF (UPDATE(HostURL))
			BEGIN
               INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'HostURL',
							  D.HostURL,
							  I.HostURL,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
			     END 
			END 

		END	   
						
	ELSE IF @NoOfRows > 1
				BEGIN
		
					SET @LoopIndex = 1
					--get the next row id
					SELECT @NextRowId = Min(Inserted.Id) From Inserted 
					
		WHILE @NoOfRows >= @LoopIndex
					BEGIN
					SELECT @CurRowId=I.ID FROM 	Inserted  I WHERE  ID = @NextRowId	
						
		IF ((SELECT COUNT(*) FROM CarMakes AS CM WITH (NOLOCK)
						JOIN deleted AS I ON CM.ID=I.ID WHERE  I.ID = @NextRowId)=0)
			 BEGIN 
		 
		 INSERT INTO  CarWaleMasterDataLogs
											(TableName,
											 AffectedId,
											 Remarks,
											 ColumnName,
											 OldValue,
											 NewValue,
											 CreatedOn
											)
		  SELECT 'CarMakes',
				  ID,
				  'Record Inserted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
				FROM inserted										 
		         WHERE  ID = @NextRowId	
		 END               
	ELSE 
	   BEGIN 
				   
				   IF (UPDATE(Name))
			BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														)
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Name',
							  D.Name,
							  I.Name,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END 
			IF (UPDATE(IsDeleted))
			BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'IsDeleted',
							  D.IsDeleted,
							  I.IsDeleted,
							  GETDATE()
						    FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END 
			
			IF (UPDATE(LogoUrl))
		BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'LogoUrl',
							  D.LogoUrl,
							  I.LogoUrl,
							  GETDATE()
							FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END 
	
      IF (UPDATE(Used))
		BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														)
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Used',
							  D.Used,
							  I.Used,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END 
     IF (UPDATE(New))
	BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														)
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'New',
							  D.New,
							  I.New,
							  GETDATE()
							FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END
      IF (UPDATE(Indian))
	BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Indian',
							  D.Indian,
							  I.Indian,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END
			IF (UPDATE(Imported))
			BEGIN
				 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Imported',
							  D.Imported,
							  I.Imported,
							  GETDATE()
							FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END 
			IF (UPDATE(Futuristic))
			BEGIN
			 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														)
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Futuristic',
							  D.Futuristic,
							  I.Futuristic,
							  GETDATE()
							FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END 
			IF (UPDATE(Classic))
			BEGIN
			 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'Classic',
							  D.Classic,
							  I.Classic,
							  GETDATE()
						FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END
			IF (UPDATE(IsReplicated))
			BEGIN
			 INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														)
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'IsReplicated',
							  D.IsReplicated,
							  I.IsReplicated,
							  GETDATE()
							FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			END 
			IF (UPDATE(HostURL))
			BEGIN
               INSERT INTO  CarWaleMasterDataLogs
														(TableName,
														 AffectedId,
														 Remarks,
														 ColumnName,
														 OldValue,
														 NewValue,
														 CreatedOn
														 )
					  SELECT 'CarMakes',
							  I.ID,
							  'Record Updated',
							  'HostURL',
							  D.HostURL,
							  I.HostURL,
							  GETDATE()
							FROM inserted AS I 
							 JOIN Deleted AS D ON I.ID=D.ID
							 WHERE  I.ID = @NextRowId	
			     END 
			END 

						--increase the counter
						SET @LoopIndex = @LoopIndex + 1
						--Also reset the next row id
						SELECT @NextRowId = MIN(ID) FROM Inserted WHERE ID > @CurRowId
					END	--end of while

				END--end of else for no of rows

END

GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in CarMakes Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCarMakesDeleteLogs]
   ON [dbo].[CarMakes]
   FOR Delete
AS 
BEGIN
		 
		 INSERT INTO  CarWaleMasterDataLogs
											(TableName,
											 AffectedId,
											 Remarks,
											 ColumnName,
											 OldValue,
											 NewValue,
											 CreatedOn
											 )
		  SELECT 'CarMakes',
				  ID,
				  'Record Deleted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM Deleted										 
 END
