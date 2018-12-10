CREATE TABLE [dbo].[States] (
    [ID]               NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]             VARCHAR (30) NOT NULL,
    [StateCode]        VARCHAR (2)  NULL,
    [IsDeleted]        BIT          CONSTRAINT [DF_States_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsUniversal]      BIT          CONSTRAINT [DF_States_IsUniversal] DEFAULT ((0)) NOT NULL,
    [StateEntryDate]   DATETIME     CONSTRAINT [DF_States_StateEntryDate] DEFAULT (getdate()) NULL,
    [StateMaskingName] VARCHAR (40) NULL,
    [StateLattitude]   FLOAT (53)   NULL,
    [StateLongitude]   FLOAT (53)   NULL,
    CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in States Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigStatesLogs]
   ON [dbo].[States]
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
		IF ((SELECT COUNT(*) FROM States AS CM WITH (NOLOCK)
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
		  SELECT 'States',
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
			IF ( Update(name) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'States', 
             I.id, 
             'Record Updated', 
             'Name', 
             D.name, 
             I.name, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(statecode) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'States', 
             I.id, 
             'Record Updated', 
             'StateCode', 
             D.statecode, 
             I.statecode, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(isdeleted) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'States', 
             I.id, 
             'Record Updated', 
             'IsDeleted', 
             D.isdeleted, 
             I.isdeleted, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(isuniversal) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'States', 
             I.id, 
             'Record Updated', 
             'IsUniversal', 
             D.isuniversal, 
             I.isuniversal, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
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
						
		IF ((SELECT COUNT(*) FROM States AS CM WITH (NOLOCK)
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
		  SELECT 'States',
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
			IF ( Update(name) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'States', 
             I.id, 
             'Record Updated', 
             'Name', 
             D.name, 
             I.name, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(statecode) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'States', 
             I.id, 
             'Record Updated', 
             'StateCode', 
             D.statecode, 
             I.statecode, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(isdeleted) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'States', 
             I.id, 
             'Record Updated', 
             'IsDeleted', 
             D.isdeleted, 
             I.isdeleted, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(isuniversal) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'States', 
             I.id, 
             'Record Updated', 
             'IsUniversal', 
             D.isuniversal, 
             I.isuniversal, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
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
---Description: This trigger will maintain the logs for updataion, deletion and insertion in States Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigStatesDeleteLogs]
   ON [dbo].[States]
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
		  SELECT 'States',
				  ID,
				  'Record Deleted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM Deleted										 
 END
