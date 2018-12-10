CREATE TABLE [dbo].[TC_InquiryStatus] (
    [Id]                 INT          IDENTITY (1, 1) NOT NULL,
    [Status]             VARCHAR (50) NOT NULL,
    [IsActive]           BIT          CONSTRAINT [DF_TC_InquiryStatus_IsActive] DEFAULT ((1)) NOT NULL,
    [TC_InquiryStatusId] SMALLINT     NULL,
    CONSTRAINT [PK_TC_InquiryStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
----------------------------------------------------------------------------------------------------------------
---Created By: Manish Chourasiya
---Created On: 15-05-2013
---Description: This trigger will maintain the logs for updataion, deletion and insertion in TC_InquiryStatus Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigTC_InquiryStatusDeleteLogs]
   ON [dbo].[TC_InquiryStatus]
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
		  SELECT 'TC_InquiryStatus',
				  TC_InquiryStatusId,
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
---Description: This trigger will maintain the logs for updataion, deletion and insertion in TC_InquiryStatus Table.
----------------------------------------------------------------------------------------------------------------
CREATE TRIGGER [dbo].[TrigTC_InquiryStatusLogs]
   ON [dbo].[TC_InquiryStatus]
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
		IF ((SELECT COUNT(*) FROM TC_InquiryStatus AS CM WITH (NOLOCK)
						JOIN deleted AS I ON CM.TC_InquiryStatusID=I.TC_InquiryStatusID)=0)
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
		  SELECT 'TC_InquiryStatus',
				  TC_InquiryStatusId,
				  'Record Inserted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
		   FROM inserted										 
		  
		 END               
	ELSE 
	   BEGIN 
	   IF ( Update(status) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquiryStatus', 
             I.TC_InquiryStatusId, 
             'Record Updated', 
             'Status', 
             D.status, 
             I.status, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.TC_InquiryStatusId = D.TC_InquiryStatusId
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
      SELECT 'TC_InquiryStatus', 
             I.TC_InquiryStatusId, 
             'Record Updated', 
             'IsActive', 
             D.isactive, 
             I.isactive, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.TC_InquiryStatusId = D.TC_InquiryStatusId 
  END 
		END	
		END   
						
	ELSE IF @NoOfRows > 1
				BEGIN
		
					SET @LoopIndex = 1
					--get the next row id
					SELECT @NextRowId = Min(Inserted.TC_InquiryStatusId) From Inserted 
					
		WHILE @NoOfRows >= @LoopIndex
					BEGIN
					SELECT @CurRowId=I.TC_InquiryStatusId FROM 	Inserted  I WHERE  TC_InquiryStatusId = @NextRowId	
						
		IF ((SELECT COUNT(*) FROM TC_InquiryStatus AS CM WITH (NOLOCK)
						JOIN deleted AS I ON CM.TC_InquiryStatusId=I.TC_InquiryStatusId WHERE  I.TC_InquiryStatusId = @NextRowId)=0)
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
		  SELECT 'TC_InquiryStatus',
				  TC_InquiryStatusId,
				  'Record Inserted',
				  NULL,
				  NULL,
				  NULL,
				  GETDATE()
				FROM inserted										 
		         WHERE  TC_InquiryStatusId = @NextRowId	
		 END               
	ELSE 
	   BEGIN 
	     IF ( Update(status) ) 
  BEGIN 
      INSERT INTO carwalemasterdatalogs 
                  (tablename, 
                   affectedid, 
                   remarks, 
                   columnname, 
                   oldvalue, 
                   newvalue, 
                   createdon) 
      SELECT 'TC_InquiryStatus', 
             I.TC_InquiryStatusId, 
             'Record Updated', 
             'Status', 
             D.status, 
             I.status, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.TC_InquiryStatusId = D.TC_InquiryStatusId 
      WHERE  I.TC_InquiryStatusId = @NextRowId 
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
      SELECT 'TC_InquiryStatus', 
             I.TC_InquiryStatusId, 
             'Record Updated', 
             'IsActive', 
             D.isactive, 
             I.isactive, 
             Getdate() 
      FROM   inserted AS I 
             JOIN deleted AS D 
               ON I.TC_InquiryStatusId = D.TC_InquiryStatusId 
      WHERE  I.TC_InquiryStatusId = @NextRowId 
  END 
			END 

						--increase the counter
						SET @LoopIndex = @LoopIndex + 1
						--Also reset the next row id
						SELECT @NextRowId = MIN(TC_InquiryStatusId) FROM Inserted WHERE TC_InquiryStatusId > @CurRowId
					END	--end of while

				END--end of else for no of rows

END
