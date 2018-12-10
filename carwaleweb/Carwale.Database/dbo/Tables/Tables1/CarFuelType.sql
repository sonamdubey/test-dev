CREATE TABLE [dbo].[CarFuelType] (
    [FuelTypeId] SMALLINT     IDENTITY (1, 1) NOT NULL,
    [FuelType]   VARCHAR (20) NULL
);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in CarFuelType Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCarFuelTypeDeleteLogs]
   ON [dbo].[CarFuelType]
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
		  SELECT 'CarFuelType',
				  FuelTypeId,
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
---Description: This trigger will maintain the logs for updataion, deletion and insertion in CarFuelType Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCarFuelTypeLogs]
   ON [dbo].[CarFuelType]
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
		IF  ((SELECT count(*) FROM CarFuelType AS CM WITH (NOLOCK)
						JOIN deleted AS I ON CM.FuelTypeId=I.FuelTypeId)=0)
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
		  SELECT 'CarFuelType',
				  FuelTypeId,
				  'Record Inserted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM inserted										 
		  
		 END               
	ELSE 
	   BEGIN
	   IF ( Update(fueltypeid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarFuelType', 
             I.FuelTypeId, 
             'Record Updated', 
             'FuelTypeId', 
             D.fueltypeid, 
             I.fueltypeid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.FuelTypeId = D.FuelTypeId
  END 

IF ( Update(fueltype) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarFuelType', 
             I.FuelTypeId, 
             'Record Updated', 
             'FuelType', 
             D.fueltype, 
             I.fueltype, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.FuelTypeId = D.FuelTypeId
  END  
		END	
		END   
						
	ELSE IF @NoOfRows > 1
				BEGIN
		
					SET @LoopIndex = 1
					--get the next row id
					SELECT @NextRowId = Min(Inserted.FuelTypeId) From Inserted 
					
		WHILE @NoOfRows >= @LoopIndex
					BEGIN
					SELECT @CurRowId=I.FuelTypeId FROM 	Inserted  I WHERE  FuelTypeId = @NextRowId	
						
		IF ((SELECT COUNT(*) FROM CarFuelType AS CM WITH (NOLOCK)
						JOIN deleted AS I ON CM.FuelTypeId=I.FuelTypeId WHERE  I.FuelTypeId = @NextRowId)=0)
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
		  SELECT 'CarFuelType',
				  FuelTypeId,
				  'Record Inserted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
				FROM inserted										 
		         WHERE  FuelTypeId = @NextRowId	
		 END               
	ELSE 
	   BEGIN 
			IF ( Update(fueltypeid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarFuelType', 
             I.FuelTypeId, 
             'Record Updated', 
             'FuelTypeId', 
             D.fueltypeid, 
             I.fueltypeid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.FuelTypeId = D.FuelTypeId
      WHERE  I.FuelTypeId = @NextRowId 
  END 

IF ( Update(fueltype) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarFuelType', 
             I.FuelTypeId, 
             'Record Updated', 
             'FuelType', 
             D.fueltype, 
             I.fueltype, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.FuelTypeId = D.FuelTypeId
      WHERE  I.FuelTypeId = @NextRowId 
  END 
			END 

						--increase the counter
						SET @LoopIndex = @LoopIndex + 1
						--Also reset the next row id
						SELECT @NextRowId = MIN(FuelTypeId) FROM Inserted WHERE FuelTypeId > @CurRowId
					END	--end of while

				END--end of else for no of rows

END
