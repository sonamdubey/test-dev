CREATE TABLE [dbo].[TC_InquirySource] (
    [Id]                      INT           NOT NULL,
    [Source]                  VARCHAR (100) NOT NULL,
    [IsActive]                BIT           CONSTRAINT [DF_TC_Source_IsActive] DEFAULT ((1)) NOT NULL,
    [ChangeSourceId]          BIT           CONSTRAINT [DF_TC_InquirySource_ChangeSourceId] DEFAULT ((1)) NOT NULL,
    [IsVisible]               BIT           NULL,
    [IsVisibleCW]             BIT           NULL,
    [IsVisibleBW]             BIT           NULL,
    [TC_InquiryGroupSourceId] SMALLINT      NULL,
    [IsVisibleForExcelSheet]  BIT           CONSTRAINT [DF__TC_Inquir__IsVis__2C24DFEF] DEFAULT ((0)) NOT NULL,
    [MakeId]                  INT           NULL,
    [orderby]                 TINYINT       NULL,
    [Product_NewCar]          TINYINT       NULL,
    CONSTRAINT [PK_TC_Source] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_InquirySource_TC_InquiryGroupSourceId]
    ON [dbo].[TC_InquirySource]([TC_InquiryGroupSourceId] ASC);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in TC_InquirySource Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigTC_InquirySourceLogs]
   ON dbo.TC_InquirySource
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
		IF ((SELECT count(*) FROM TC_InquirySource AS CM WITH (NOLOCK)
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
		  SELECT 'TC_InquirySource',
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
         IF ( Update(source) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquirySource', 
             I.id, 
             'Record Updated', 
             'Source', 
             D.source, 
             I.source, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(isactive) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquirySource', 
             I.id, 
             'Record Updated', 
             'IsActive', 
             D.isactive, 
             I.isactive, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(changesourceid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquirySource', 
             I.id, 
             'Record Updated', 
             'ChangeSourceId', 
             D.changesourceid, 
             I.changesourceid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
  END 

IF ( Update(isvisible) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquirySource', 
             I.id, 
             'Record Updated', 
             'IsVisible', 
             D.isvisible, 
             I.isvisible, 
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
						
		IF ((SELECT COUNT(*) FROM TC_InquirySource AS CM WITH (NOLOCK)
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
		  SELECT 'TC_InquirySource',
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
	IF ( Update(source) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquirySource', 
             I.id, 
             'Record Updated', 
             'Source', 
             D.source, 
             I.source, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(isactive) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquirySource', 
             I.id, 
             'Record Updated', 
             'IsActive', 
             D.isactive, 
             I.isactive, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(changesourceid) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquirySource', 
             I.id, 
             'Record Updated', 
             'ChangeSourceId', 
             D.changesourceid, 
             I.changesourceid, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.id = D.id 
      WHERE  I.id = @NextRowId 
  END 

IF ( Update(isvisible) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquirySource', 
             I.id, 
             'Record Updated', 
             'IsVisible', 
             D.isvisible, 
             I.isvisible, 
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
---Description: This trigger will maintain the logs for updataion, deletion and insertion in TC_InquirySource Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigTC_InquirySourceDeleteLogs]
   ON dbo.TC_InquirySource
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
		  SELECT 'TC_InquirySource',
				  ID,
				  'Record Deleted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM Deleted										 
 END
