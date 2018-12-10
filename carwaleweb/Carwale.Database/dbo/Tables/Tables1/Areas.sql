CREATE TABLE [dbo].[Areas] (
    [ID]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (50)    NOT NULL,
    [CityId]        NUMERIC (18)    NOT NULL,
    [PinCode]       VARCHAR (10)    NULL,
    [Lattitude]     DECIMAL (18, 6) NULL,
    [Longitude]     DECIMAL (18, 6) NULL,
    [IsDeleted]     NUMERIC (18)    CONSTRAINT [DF_Areas_IsDeleted] DEFAULT (0) NOT NULL,
    [AreaEntryDate] DATETIME        CONSTRAINT [DF_Areas_AreaEntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Areas] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Areas_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([ID]) ON UPDATE CASCADE
);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in Areas Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigAreasDeleteLogs]
   ON [dbo].[Areas]
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
		  SELECT 'Areas',
				  ID,
				  'Record Deleted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM Deleted										 
 END

GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in Areas Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigAreasLogs]
   ON [dbo].[Areas]
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
	
	

	
		IF ((SELECT COUNT(*) FROM Areas AS CM WITH (NOLOCK)
						JOIN DELETED AS I ON CM.ID=I.ID)=0)
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
		  SELECT 'Areas',
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
      SELECT 'Areas', 
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

IF ( Update(cityid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Areas', 
             I.id, 
             'Record Updated', 
             'CityId', 
             D.cityid, 
             I.cityid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(pincode) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Areas', 
             I.id, 
             'Record Updated', 
             'PinCode', 
             D.pincode, 
             I.pincode, 
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
      SELECT 'Areas', 
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
      SELECT 'Areas', 
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
      SELECT 'Areas', 
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
						
		IF  ((SELECT COUNT(*) FROM Areas AS CM WITH (NOLOCK)
						JOIN DELETED AS I ON CM.ID=I.ID WHERE  I.ID = @NextRowId)=0)
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
		  SELECT 'Areas',
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
      SELECT 'Areas', 
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

IF ( Update(cityid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Areas', 
             I.id, 
             'Record Updated', 
             'CityId', 
             D.cityid, 
             I.cityid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(pincode) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Areas', 
             I.id, 
             'Record Updated', 
             'PinCode', 
             D.pincode, 
             I.pincode, 
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
      SELECT 'Areas', 
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
      SELECT 'Areas', 
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
      SELECT 'Areas', 
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
				    
			END 

						--increase the counter
						SET @LoopIndex = @LoopIndex + 1
						--Also reset the next row id
						SELECT @NextRowId = MIN(ID) FROM Inserted WHERE ID > @CurRowId
					END	--end of while

				END--end of else for no of rows

END
