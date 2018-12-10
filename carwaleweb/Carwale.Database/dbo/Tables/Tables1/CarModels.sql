CREATE TABLE [dbo].[CarModels] (
    [ID]                   NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [Name]                 VARCHAR (30)    NOT NULL,
    [CarMakeId]            NUMERIC (18)    NOT NULL,
    [IsDeleted]            BIT             CONSTRAINT [DF_CarModels_IsDeleted_1] DEFAULT ((0)) NOT NULL,
    [Used]                 BIT             CONSTRAINT [DF_CarModels_Used_1] DEFAULT ((1)) NOT NULL,
    [New]                  BIT             CONSTRAINT [DF_CarModels_New_1] DEFAULT ((1)) NOT NULL,
    [Indian]               BIT             CONSTRAINT [DF_CarModels_Indian_1] DEFAULT ((1)) NOT NULL,
    [Imported]             BIT             CONSTRAINT [DF_CarModels_Imported_1] DEFAULT ((0)) NOT NULL,
    [Futuristic]           BIT             CONSTRAINT [DF_CarModels_Futuristic_1] DEFAULT ((0)) NOT NULL,
    [Classic]              BIT             CONSTRAINT [DF_CarModels_Classic_1] DEFAULT ((0)) NOT NULL,
    [Modified]             BIT             CONSTRAINT [DF_CarModels_Modified_1] DEFAULT ((0)) NOT NULL,
    [ReviewRate]           DECIMAL (18, 2) CONSTRAINT [DF_CarModels_ReviewRate_1] DEFAULT ((0)) NULL,
    [ReviewCount]          NUMERIC (18)    CONSTRAINT [DF_CarModels_ReviewCount_1] DEFAULT ((0)) NULL,
    [Looks]                DECIMAL (18, 2) NULL,
    [Performance]          DECIMAL (18, 2) NULL,
    [Comfort]              DECIMAL (18, 2) NULL,
    [ValueForMoney]        DECIMAL (18, 2) NULL,
    [FuelEconomy]          DECIMAL (18, 2) NULL,
    [SmallPic]             VARCHAR (200)   NULL,
    [LargePic]             VARCHAR (200)   NULL,
    [MoCreatedOn]          DATETIME        NULL,
    [MoUpdatedBy]          NUMERIC (18)    NULL,
    [MoUpdatedOn]          DATETIME        NULL,
    [IsReplicated]         BIT             CONSTRAINT [DF__CarModels__IsRep_1] DEFAULT ((0)) NULL,
    [HostURL]              VARCHAR (100)   DEFAULT ('img.carwale.com') NULL,
    [comment]              VARCHAR (5000)  NULL,
    [Discontinuation_date] DATETIME        NULL,
    [ReplacedByModelId]    SMALLINT        NULL,
    [DiscontinuationId]    INT             NULL,
    [MinPrice]             INT             NULL,
    [MaxPrice]             INT             NULL,
    [CarVersionID_Top]     INT             NULL,
    [SubSegmentID]         INT             NULL,
    [Summary]              NVARCHAR (MAX)  NULL,
    [MaskingName]          VARCHAR (50)    NULL,
    [RootId]               SMALLINT        NULL,
    [Platform]             VARCHAR (500)   NULL,
    [Generation]           TINYINT         NULL,
    [Upgrade]              TINYINT         NULL,
    [ModelLaunchDate]      DATETIME        NULL,
    [UsedCarRating]        FLOAT (53)      NULL,
    [VideoCount]           INT             NULL,
    [ModelBodyStyle]       TINYINT         NULL,
    [XLargePic]            VARCHAR (150)   NULL,
    [OriginalImgPath]      VARCHAR (150)   NULL,
    [isSolidColor]         BIT             DEFAULT ((1)) NOT NULL,
    [isMetallicColor]      BIT             DEFAULT ((0)) NOT NULL,
    [ModelPopularity]      INT             DEFAULT ((0)) NULL,
    [MinAvgPrice]          INT             NULL,
    CONSTRAINT [PK_CarModels_1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UK_carmodels_MaskingName_1] UNIQUE NONCLUSTERED ([MaskingName] ASC)
);


GO
CREATE NONCLUSTERED INDEX [Idx_nc_CarModels_New]
    ON [dbo].[CarModels]([New] ASC)
    INCLUDE([ID]);


GO
CREATE NONCLUSTERED INDEX [IX_carmodels]
    ON [dbo].[CarModels]([CarMakeId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_carmodels_RootId]
    ON [dbo].[CarModels]([RootId] ASC);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in CarModels Table.
---Modified By: Purohith Gugugloth on  13th oct,2016
---     Reason: Added Two columns MaskingName and OriginalImgPath 
---Modified By: Saket Thapliyal on  8th nov,2016
---     Reason: Added Two columns RootId and SubSegmentId 
---------------------------------------------------------------------------------------------------------------s-
CREATE TRIGGER [dbo].[TrigCarModelsLogs]
   ON [dbo].[CarModels]
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
		IF ((SELECT COUNT(*) FROM CarModels AS CM WITH (NOLOCK)  
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
		  SELECT 'CarModels',
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
	
IF ( Update(MaskingName) )               ---Modified By: Purohith Gugugloth on  13th oct,2016
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'MaskingName', 
             D.MaskingName, 
             I.MaskingName, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

  IF ( Update(RootId) )               ---Modified By: Saket Thapliyal on  8th nov,2016
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'RootId', 
             D.RootId, 
             I.RootId, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

    IF ( Update(SubSegmentId) )               ---Modified By: Saket Thapliyal on  8th nov,2016
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'SubSegmentId', 
             D.SubSegmentId, 
             I.SubSegmentId, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 
  
  IF ( Update(OriginalImgPath) )          ---Modified By: Purohith Gugugloth on  13th oct,2016
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'OriginalImgPath', 
             D.OriginalImgPath, 
             I.OriginalImgPath, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 
  				   
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
      SELECT 'CarModels', 
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

IF ( Update(carmakeid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'CarMakeId', 
             D.carmakeid, 
             I.carmakeid, 
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
      SELECT 'CarModels', 
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

IF ( Update(used) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Used', 
             D.used, 
             I.used, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(new) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'New', 
             D.new, 
             I.new, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(indian) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Indian', 
             D.indian, 
             I.indian, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(imported) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Imported', 
             D.imported, 
             I.imported, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(futuristic) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Futuristic', 
             D.futuristic, 
             I.futuristic, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(classic) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Classic', 
             D.classic, 
             I.classic, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(modified) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Modified', 
             D.modified, 
             I.modified, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(reviewrate) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'ReviewRate', 
             D.reviewrate, 
             I.reviewrate, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(reviewcount) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'ReviewCount', 
             D.reviewcount, 
             I.reviewcount, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(looks) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Looks', 
             D.looks, 
             I.looks, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(performance) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Performance', 
             D.performance, 
             I.performance, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(comfort) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Comfort', 
             D.comfort, 
             I.comfort, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(valueformoney) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'ValueForMoney', 
             D.valueformoney, 
             I.valueformoney, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(fueleconomy) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'FuelEconomy', 
             D.fueleconomy, 
             I.fueleconomy, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(smallpic) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'SmallPic', 
             D.smallpic, 
             I.smallpic, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(largepic) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'LargePic', 
             D.largepic, 
             I.largepic, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(isreplicated) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'IsReplicated', 
             D.isreplicated, 
             I.isreplicated, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(hosturl) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'HostURL', 
             D.hosturl, 
             I.hosturl, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(comment) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'comment', 
             D.comment, 
             I.comment, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(discontinuation_date) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Discontinuation_date', 
             D.discontinuation_date, 
             I.discontinuation_date, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(replacedbymodelid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'ReplacedByModelId', 
             D.replacedbymodelid, 
             I.replacedbymodelid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(discontinuationid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'DiscontinuationId', 
             D.discontinuationid, 
             I.discontinuationid, 
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
						
			IF ((SELECT COUNT(*) FROM CarModels AS CM WITH (NOLOCK) 
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
					  SELECT 'CarModels',
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
				   
				  IF ( Update(MaskingName) )            ---Modified By: Purohith Gugugloth on  13th oct,2016
				  BEGIN 
					  INSERT INTO carwalemasterdatalogs 
								  (tablename, 
								   affectedid, 
								   remarks, 
								   columnname, 
								   oldvalue, 
								   newvalue, 
								   createdon) 
					  SELECT 'CarModels', 
							 I.id, 
							 'Record Updated', 
							 'MaskingName', 
							 D.MaskingName, 
							 I.MaskingName, 
							 Getdate() 
					  FROM   inserted AS I 
							 JOIN deleted AS D 
							   ON I.id = D.id 
					  WHERE  I.id = @NextRowId 
				  END 		  
				  
				  IF ( Update(OriginalImgPath) )             ---Modified By: Purohith Gugugloth on  13th oct,2016
				  BEGIN 
					  INSERT INTO carwalemasterdatalogs 
								  (tablename, 
								   affectedid, 
								   remarks, 
								   columnname, 
								   oldvalue, 
								   newvalue, 
								   createdon) 
					  SELECT 'CarModels', 
							 I.id, 
							 'Record Updated', 
							 'OriginalImgPath', 
							 D.OriginalImgPath, 
							 I.OriginalImgPath, 
							 Getdate() 
					  FROM   inserted AS I 
							 JOIN deleted AS D 
							   ON I.id = D.id 
					  WHERE  I.id = @NextRowId 
				  END 

				    IF ( Update(RootId) )               ---Modified By: Saket Thapliyal on  8th nov,2016
					BEGIN 
						 INSERT INTO carwalemasterdatalogs 
									 (tablename, 
								      affectedid, 
									  remarks, 
									  columnname, 
									  oldvalue, 
									  newvalue, 
									  createdon) 
						  SELECT 'CarModels', 
								 I.id, 
								 'Record Updated', 
								 'RootId', 
								 D.RootId, 
								 I.RootId, 
								 Getdate() 
						  FROM   inserted AS I 
								 JOIN deleted AS D 
								   ON I.id = D.id 
					  END 

						IF ( Update(SubSegmentId) )               ---Modified By: Saket Thapliyal on  8th nov,2016
						BEGIN 
						  INSERT INTO carwalemasterdatalogs 
									  (tablename, 
									   affectedid, 
									   remarks, 
									   columnname, 
									   oldvalue, 
									   newvalue, 
									   createdon) 
						  SELECT 'CarModels', 
								 I.id, 
								 'Record Updated', 
								 'SubSegmentId', 
								 D.SubSegmentId, 
								 I.SubSegmentId, 
								 Getdate() 
						  FROM   inserted AS I 
								 JOIN deleted AS D 
								   ON I.id = D.id 
					  END 

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
      SELECT 'CarModels', 
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

					IF ( Update(carmakeid) ) 
					  BEGIN 
						  INSERT INTO carwalemasterdatalogs 
									  (tablename, 
									   affectedid, 
									   remarks, 
									   columnname, 
									   oldvalue, 
									   newvalue, 
									   createdon) 
						  SELECT 'CarModels', 
								 I.id, 
								 'Record Updated', 
								 'CarMakeId', 
								 D.carmakeid, 
								 I.carmakeid, 
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
							  SELECT 'CarModels', 
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

						IF ( Update(used) ) 
						  BEGIN 
							  INSERT INTO carwalemasterdatalogs 
										  (tablename, 
										   affectedid, 
										   remarks, 
										   columnname, 
										   oldvalue, 
										   newvalue, 
										   createdon) 
							  SELECT 'CarModels', 
									 I.id, 
									 'Record Updated', 
									 'Used', 
									 D.used, 
									 I.used, 
									 Getdate() 
							  FROM   inserted AS I 
									 JOIN deleted AS D 
									   ON I.id = D.id 
							  WHERE  I.id = @NextRowId 
						  END 

							IF ( Update(new) ) 
							  BEGIN 
								  INSERT INTO carwalemasterdatalogs 
											  (tablename, 
											   affectedid, 
											   remarks, 
											   columnname, 
											   oldvalue, 
											   newvalue, 
											   createdon) 
								  SELECT 'CarModels', 
										 I.id, 
										 'Record Updated', 
										 'New', 
										 D.new, 
										 I.new, 
										 Getdate() 
								  FROM   inserted AS I 
										 JOIN deleted AS D 
										   ON I.id = D.id 
								  WHERE  I.id = @NextRowId 
							  END 

							IF ( Update(indian) ) 
							  BEGIN 
								  INSERT INTO carwalemasterdatalogs 
											  (tablename, 
											   affectedid, 
											   remarks, 
											   columnname, 
											   oldvalue, 
											   newvalue, 
											   createdon) 
								  SELECT 'CarModels', 
										 I.id, 
										 'Record Updated', 
										 'Indian', 
										 D.indian, 
										 I.indian, 
										 Getdate() 
								  FROM   inserted AS I 
										 JOIN deleted AS D 
										   ON I.id = D.id 
								  WHERE  I.id = @NextRowId 
							  END 

							IF ( Update(imported) ) 
							  BEGIN 
								  INSERT INTO carwalemasterdatalogs 
											  (tablename, 
											   affectedid, 
											   remarks, 
											   columnname, 
											   oldvalue, 
											   newvalue, 
											   createdon) 
								  SELECT 'CarModels', 
										 I.id, 
										 'Record Updated', 
										 'Imported', 
										 D.imported, 
										 I.imported, 
										 Getdate() 
								  FROM   inserted AS I 
										 JOIN deleted AS D 
										   ON I.id = D.id 
								  WHERE  I.id = @NextRowId 
							  END 

							IF ( Update(futuristic) ) 
							  BEGIN 
								  INSERT INTO carwalemasterdatalogs 
											  (tablename, 
											   affectedid, 
											   remarks, 
											   columnname, 
											   oldvalue, 
											   newvalue, 
											   createdon) 
								  SELECT 'CarModels', 
										 I.id, 
										 'Record Updated', 
										 'Futuristic', 
										 D.futuristic, 
										 I.futuristic, 
										 Getdate() 
								  FROM   inserted AS I 
										 JOIN deleted AS D 
										   ON I.id = D.id 
								  WHERE  I.id = @NextRowId 
							  END 

							IF ( Update(classic) ) 
							  BEGIN 
								  INSERT INTO carwalemasterdatalogs 
											  (tablename, 
											   affectedid, 
											   remarks, 
											   columnname, 
											   oldvalue, 
											   newvalue, 
											   createdon) 
								  SELECT 'CarModels', 
										 I.id, 
										 'Record Updated', 
										 'Classic', 
										 D.classic, 
										 I.classic, 
										 Getdate() 
								  FROM   inserted AS I 
										 JOIN deleted AS D 
										   ON I.id = D.id 
								  WHERE  I.id = @NextRowId 
							  END 

							IF ( Update(modified) ) 
							  BEGIN 
								  INSERT INTO carwalemasterdatalogs 
											  (tablename, 
											   affectedid, 
											   remarks, 
											   columnname, 
											   oldvalue, 
											   newvalue, 
											   createdon) 
								  SELECT 'CarModels', 
										 I.id, 
										 'Record Updated', 
										 'Modified', 
										 D.modified, 
										 I.modified, 
										 Getdate() 
								  FROM   inserted AS I 
										 JOIN deleted AS D 
										   ON I.id = D.id 
								  WHERE  I.id = @NextRowId 
							  END 

							IF ( Update(reviewrate) ) 
							  BEGIN 
								  INSERT INTO carwalemasterdatalogs 
											  (tablename, 
											   affectedid, 
											   remarks, 
											   columnname, 
											   oldvalue, 
											   newvalue, 
											   createdon) 
								  SELECT 'CarModels', 
										 I.id, 
										 'Record Updated', 
										 'ReviewRate', 
										 D.reviewrate, 
										 I.reviewrate, 
										 Getdate() 
								  FROM   inserted AS I 
										 JOIN deleted AS D 
										   ON I.id = D.id 
								  WHERE  I.id = @NextRowId 
							  END 

IF ( Update(reviewcount) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'ReviewCount', 
             D.reviewcount, 
             I.reviewcount, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(looks) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Looks', 
             D.looks, 
             I.looks, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(performance) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Performance', 
             D.performance, 
             I.performance, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(comfort) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Comfort', 
             D.comfort, 
             I.comfort, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(valueformoney) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'ValueForMoney', 
             D.valueformoney, 
             I.valueformoney, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(fueleconomy) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'FuelEconomy', 
             D.fueleconomy, 
             I.fueleconomy, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(smallpic) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'SmallPic', 
             D.smallpic, 
             I.smallpic, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(largepic) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'LargePic', 
             D.largepic, 
             I.largepic, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(isreplicated) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'IsReplicated', 
             D.isreplicated, 
             I.isreplicated, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(hosturl) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'HostURL', 
             D.hosturl, 
             I.hosturl, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(comment) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'comment', 
             D.comment, 
             I.comment, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(discontinuation_date) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'Discontinuation_date', 
             D.discontinuation_date, 
             I.discontinuation_date, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(replacedbymodelid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'ReplacedByModelId', 
             D.replacedbymodelid, 
             I.replacedbymodelid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(discontinuationid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'CarModels', 
             I.id, 
             'Record Updated', 
             'DiscontinuationId', 
             D.discontinuationid, 
             I.discontinuationid, 
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
---Description: This trigger will maintain the logs for updataion, deletion and insertion in CarModels Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCarModelsDeleteLogs]
   ON [dbo].[CarModels]
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
		  SELECT 'CarModels',
				  ID,
				  'Record Deleted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM Deleted										 
 END
