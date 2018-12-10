CREATE TABLE [dbo].[CarVersions] (
    [ID]                   NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]                 VARCHAR (50)    NULL,
    [CarModelId]           NUMERIC (18)    NOT NULL,
    [SegmentId]            NUMERIC (18)    NOT NULL,
    [BodyStyleId]          NUMERIC (18)    NOT NULL,
    [smallPic]             VARCHAR (150)   NULL,
    [largePic]             VARCHAR (150)   NULL,
    [IsDeleted]            BIT             CONSTRAINT [DF_CarVersions_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Used]                 BIT             CONSTRAINT [DF_CarVersions_Used] DEFAULT ((1)) NOT NULL,
    [New]                  BIT             CONSTRAINT [DF_CarVersions_New] DEFAULT ((1)) NOT NULL,
    [Indian]               BIT             CONSTRAINT [DF_CarVersions_Indian] DEFAULT ((1)) NOT NULL,
    [Imported]             BIT             CONSTRAINT [DF_CarVersions_Imported] DEFAULT ((0)) NOT NULL,
    [Futuristic]           BIT             CONSTRAINT [DF_CarVersions_Futuristic] DEFAULT ((0)) NOT NULL,
    [Classic]              BIT             CONSTRAINT [DF_CarVersions_Classic] DEFAULT ((0)) NOT NULL,
    [Modified]             BIT             CONSTRAINT [DF_CarVersions_Modified] DEFAULT ((0)) NOT NULL,
    [ReviewRate]           DECIMAL (18, 2) CONSTRAINT [DF_CarVersions_ReviewRate] DEFAULT ((0)) NULL,
    [ReviewCount]          NUMERIC (18)    CONSTRAINT [DF_CarVersions_ReviewCount] DEFAULT ((0)) NULL,
    [Looks]                DECIMAL (18, 2) NULL,
    [Performance]          DECIMAL (18, 2) NULL,
    [Comfort]              DECIMAL (18, 2) NULL,
    [ValueForMoney]        DECIMAL (18, 2) NULL,
    [FuelEconomy]          DECIMAL (18, 2) NULL,
    [SubSegmentId]         NUMERIC (18)    NULL,
    [CarFuelType]          TINYINT         NULL,
    [CarTransmission]      TINYINT         NULL,
    [VCreatedOn]           DATETIME        NULL,
    [VUpdatedBy]           NUMERIC (18)    NULL,
    [VUpdatedOn]           DATETIME        NULL,
    [IsReplicated]         BIT             CONSTRAINT [DF__CarVersio__IsRep__39B319E0] DEFAULT ((0)) NULL,
    [HostURL]              VARCHAR (100)   CONSTRAINT [DF__CarVersio__HostU__5C0831E4] DEFAULT ('img.carwale.com') NULL,
    [ReplacedByVersionId]  SMALLINT        NULL,
    [comment]              VARCHAR (5000)  NULL,
    [DiscontinuationId]    NUMERIC (18)    NULL,
    [Discontinuation_date] DATETIME        NULL,
    [IsSpecsAvailable]     BIT             CONSTRAINT [DF__CarVersio__IsSpe__0B9814CC] DEFAULT ((0)) NULL,
    [IsSpecsExist]         BIT             CONSTRAINT [DF__CarVersio__IsSpe__7A387EA0] DEFAULT ((0)) NULL,
    [SpecsSummary]         VARCHAR (100)   NULL,
    [DirPath]              VARCHAR (10)    CONSTRAINT [DF_CarVersions_DirPath] DEFAULT ('/cars/') NULL,
    [MaskingName]          VARCHAR (50)    NULL,
    [vcreatedby]           NUMERIC (18)    NULL,
    [LaunchDate]           DATETIME        NULL,
    [SpecialVersion]       BIT             CONSTRAINT [DF_CarVersions_SpecialVersion] DEFAULT ((0)) NULL,
    [OriginalImgPath]      VARCHAR (150)   NULL,
    [IsHybrid]             BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CarVersions] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CarVersions_CarBodyStyles] FOREIGN KEY ([BodyStyleId]) REFERENCES [dbo].[CarBodyStyles] ([ID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_CarVersions_CarSegments] FOREIGN KEY ([SegmentId]) REFERENCES [dbo].[CarSegments] ([ID]) ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [ix_CarVersions__CarModelId__IsDeleted]
    ON [dbo].[CarVersions]([CarModelId] ASC, [IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CarVersions_New_IsDeleted]
    ON [dbo].[CarVersions]([New] ASC, [IsDeleted] ASC)
    INCLUDE([ID], [CarModelId]);


GO
CREATE NONCLUSTERED INDEX [ix_CarVersions__IsDeleted__New__BodyStyleId]
    ON [dbo].[CarVersions]([IsDeleted] ASC, [New] ASC, [BodyStyleId] ASC)
    INCLUDE([ID], [Name], [CarModelId], [ReviewRate], [ReviewCount]);


GO
CREATE NONCLUSTERED INDEX [ix_CarVersions__SegmentId__IsDeleted__New]
    ON [dbo].[CarVersions]([SegmentId] ASC, [IsDeleted] ASC, [New] ASC)
    INCLUDE([ID], [CarModelId]);


GO
CREATE NONCLUSTERED INDEX [IX_CarVersions_SubSegmentId]
    ON [dbo].[CarVersions]([SubSegmentId] ASC)
    INCLUDE([ID], [BodyStyleId]);


GO
CREATE NONCLUSTERED INDEX [IX_CarVersions_CarFuelType]
    ON [dbo].[CarVersions]([CarFuelType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CarVersions_CarModelId]
    ON [dbo].[CarVersions]([CarModelId] ASC);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in CarVersions Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCarVersionsDeleteLogs]
   ON dbo.CarVersions
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
		  SELECT 'CarVersions',
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
---Description: This trigger will maintain the logs for updataion, deletion and insertion in CarVersions Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigCarVersionsLogs]
   ON dbo.CarVersions
   FOR INSERT,UPDATE
AS 
DECLARE @NoOfRows INT
DECLARE  @NextRowId INT
DECLARE  @LoopIndex INT
DECLARE   @CurRowId INT
BEGIN
select  @NoOfRows =COUNT(*) from inserted
	
  IF @NoOfRows = 1
	BEGIN
		IF  ((SELECT COUNT(*) FROM CarVersions AS CM WITH (NOLOCK)
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
		  SELECT 'CarVersions',
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
      SELECT 'Carversions', 
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

IF ( Update(carmodelid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'CarModelId', 
             D.carmodelid, 
             I.carmodelid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(segmentid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'SegmentId', 
             D.segmentid, 
             I.segmentid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(bodystyleid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'BodyStyleId', 
             D.bodystyleid, 
             I.bodystyleid, 
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
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
 'smallPic', 
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
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'largePic', 
             D.largepic, 
             I.largepic, 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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

IF ( Update(subsegmentid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'SubSegmentId', 
             D.subsegmentid, 
             I.subsegmentid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(carfueltype) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'CarFuelType', 
             D.carfueltype, 
             I.carfueltype, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(cartransmission) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'CarTransmission', 
             D.cartransmission, 
             I.cartransmission, 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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

IF ( Update(replacedbyversionid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'ReplacedByVersionId', 
             D.replacedbyversionid, 
             I.replacedbyversionid, 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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

IF ( Update(isspecsavailable) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'IsSpecsAvailable', 
             D.isspecsavailable, 
             I.isspecsavailable, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(isspecsexist) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'IsSpecsExist', 
             D.isspecsexist, 
             I.isspecsexist, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(specssummary) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'SpecsSummary', 
             D.specssummary, 
             I.specssummary, 
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
						
		IF  ((SELECT COUNT(*) FROM CarVersions AS CM WITH (NOLOCK)
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
		  SELECT 'CarVersions',
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
      SELECT 'Carversions', 
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

IF ( Update(carmodelid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'CarModelId', 
             D.carmodelid, 
             I.carmodelid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(segmentid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'SegmentId', 
             D.segmentid, 
             I.segmentid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(bodystyleid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'BodyStyleId', 
             D.bodystyleid, 
             I.bodystyleid, 
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
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'smallPic', 
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
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'largePic', 
             D.largepic, 
             I.largepic, 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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

IF ( Update(subsegmentid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'SubSegmentId', 
             D.subsegmentid, 
             I.subsegmentid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(carfueltype) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'CarFuelType', 
             D.carfueltype, 
             I.carfueltype, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(cartransmission) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'CarTransmission', 
             D.cartransmission, 
             I.cartransmission, 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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

IF ( Update(replacedbyversionid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'ReplacedByVersionId', 
             D.replacedbyversionid, 
             I.replacedbyversionid, 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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
      SELECT 'Carversions', 
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

IF ( Update(isspecsavailable) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
              columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'IsSpecsAvailable', 
             D.isspecsavailable, 
             I.isspecsavailable, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(isspecsexist) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'IsSpecsExist', 
             D.isspecsexist, 
             I.isspecsexist, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(specssummary) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'Carversions', 
             I.id, 
             'Record Updated', 
             'SpecsSummary', 
             D.specssummary, 
             I.specssummary, 
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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Petrol; 2=Diesel; 3=CNG;  4=LPG; 5=Electric', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CarVersions', @level2type = N'COLUMN', @level2name = N'CarFuelType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Automatic; 2=Manual;', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CarVersions', @level2type = N'COLUMN', @level2name = N'CarTransmission';

