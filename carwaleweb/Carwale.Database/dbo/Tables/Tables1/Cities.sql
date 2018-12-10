CREATE TABLE [dbo].[Cities] (
    [ID]              NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]            VARCHAR (50)    NOT NULL,
    [StateId]         NUMERIC (18)    NOT NULL,
    [IsDeleted]       BIT             CONSTRAINT [DF_Cities_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsUniversal]     BIT             CONSTRAINT [DF__Cities__IsUniver__5575A085] DEFAULT ((0)) NOT NULL,
    [Lattitude]       DECIMAL (18, 4) NULL,
    [Longitude]       DECIMAL (18, 4) NULL,
    [StdCode]         NUMERIC (18)    NULL,
    [DefaultPinCode]  VARCHAR (10)    NULL,
    [UsedCarRating]   FLOAT (53)      NULL,
    [IsPopular]       BIT             CONSTRAINT [DF_Cities_IsPopular] DEFAULT ((0)) NULL,
    [CityImageUrl]    VARCHAR (100)   NULL,
    [CityEntryDate]   DATETIME        CONSTRAINT [DF_Cities_CityEntryDate] DEFAULT (getdate()) NULL,
    [CityMaskingName] VARCHAR (60)    NULL,
    [BWCityOrder]     SMALLINT        NULL,
    [IsAreaAvailable] BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Cities_States] FOREIGN KEY ([StateId]) REFERENCES [dbo].[States] ([ID]) ON UPDATE CASCADE
);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in Cities Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCitiesLogs]
   ON [dbo].[Cities]
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
		IF ((SELECT COUNT(*) FROM Cities AS CM WITH (NOLOCK)
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
		  SELECT 'Cities',
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
      SELECT 'Cities', 
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

IF ( Update(stateid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'StateId', 
             D.stateid, 
             I.stateid, 
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
      SELECT 'Cities', 
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
      SELECT 'Cities', 
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

IF ( Update(lattitude) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'Lattitude', 
             D.lattitude, 
             I.lattitude, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(longitude) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'Longitude', 
             D.longitude, 
             I.longitude, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(stdcode) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'StdCode', 
             D.stdcode, 
             I.stdcode, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(defaultpincode) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'DefaultPinCode', 
             D.defaultpincode, 
             I.defaultpincode, 
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
						
		IF ((SELECT COUNT(*) FROM Cities AS CM WITH (NOLOCK)
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
		  SELECT 'Cities',
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
      SELECT 'Cities', 
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

IF ( Update(stateid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'StateId', 
             D.stateid, 
             I.stateid, 
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
      SELECT 'Cities', 
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
      SELECT 'Cities', 
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

IF ( Update(lattitude) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'Lattitude', 
             D.lattitude, 
             I.lattitude, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(longitude) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'Longitude', 
             D.longitude, 
             I.longitude, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(stdcode) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'StdCode', 
             D.stdcode, 
             I.stdcode, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(defaultpincode) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Cities', 
             I.id, 
             'Record Updated', 
             'DefaultPinCode', 
             D.defaultpincode, 
             I.defaultpincode, 
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
---Description: This trigger will maintain the logs for updataion, deletion and insertion in Cities Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCitiesDeleteLogs]
   ON [dbo].[Cities]
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
		  SELECT 'Cities',
				  ID,
				  'Record Deleted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM Deleted										 
 END
