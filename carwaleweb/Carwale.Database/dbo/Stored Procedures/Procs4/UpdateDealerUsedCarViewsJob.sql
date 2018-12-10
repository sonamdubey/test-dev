IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateDealerUsedCarViewsJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateDealerUsedCarViewsJob]
GO

	-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 08-09-2015
-- Description:	Load the data from text file for dealers car view count and update into DealerUsedCarViews
-- Modified By :Kundan Dombale On 23'Oct'2015
-- Description : Added INSERT INTO DealerUsedCarViewsLogs Statement to maintain the logs of table IntermediateDealerUsedCarViews 
-- Modified By : Kundan Dombale On 04-11-2015 
-- Description: Added 'eventsource' Column to DealerUsedCarViewslogs and  IntermediateDealerUsedCarViews table
-- =============================================
CREATE PROCEDURE [dbo].[UpdateDealerUsedCarViewsJob]
 AS 
    BEGIN
		
		SET NOCOUNT ON;
		
		
		BEGIN TRY
	    TRUNCATE  TABLE IntermediateDealerUsedCarViews;
	   CREATE TABLE #AllFileNames ( ID INT IDENTITY(1,1) 
	                               ,FileName varchar(150)
								  );
       DECLARE @FileName VARCHAR(255),
               @FilePath     VARCHAR(255)='L:\Cassandra\',
               @MoveFilePath     VARCHAR(255)='L:\ProcessedCassandra\',
               @SQL      VARCHAR(MAX),
               @cmdFileList      varchar(250),
               @cmdMoveFile      varchar(250),
               @WhileLoopCount INT,
               @WhileLoopControl INT=1		
               
        SET @cmdFileList='DIR /B '  + @FilePath +'*.txt'
        INSERT INTO #AllFileNames (FileName)
        EXEC Master..xp_cmdShell @cmdFileList
        
     --  SELECT * FROM #AllFileNames   WHERE FileName like '%.txt';
          
        SELECT @WhileLoopCount=COUNT(*)  
        FROM #AllFileNames
        WHERE FileName like '%.txt';
       
        WHILE (@WhileLoopCount>=@WhileLoopControl)
        BEGIN 
             SET @SQL=NULL
             SET @FileName=NULL
        
			BEGIN TRY
			
			SELECT @FileName=[FileName]
			FROM #AllFileNames
			WHERE ID=@WhileLoopControl;
			
			SET @SQL='BULK INSERT [IntermediateDealerUsedCarViews] FROM  '  +''''+@FilePath + @FileName+''''+
				'   WITH ( FIELDTERMINATOR =''|''
					  ,FIRSTROW =1
					  ,ROWTERMINATOR =''0x0A''
					  ,ROWS_PER_BATCH =5000
					 );';
					 
		    exec (@sql)
			--PRINT @sql
			 
			 SET @cmdMoveFile='move ' +@FilePath+ @FileName+'  '+@MoveFilePath+@FileName
			 
			 EXEC Master..xp_cmdShell @cmdMoveFile
			
		--	PRINT  @cmdMoveFile
			 
			 END TRY
			 BEGIN CATCH
			    INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Update Dealer Used Car Views Job',
									        'dbo.UpdateDealerUsedCarViewsJob',
											 ERROR_MESSAGE(),
											 'DealerUsedCarViews',
											 'FileName:'+ @FileName,
											 GETDATE()
                                            )
			 
			 END CATCH 
			 
			 SET @WhileLoopControl=@WhileLoopControl+1;
			 
	    END ;
		
		BEGIN								--- Added  Block to maintian the logs of IntermediateDealerUsedCarViews table 
			INSERT INTO DealerUsedCarViewslogs ( LogDate,
												ProfileId,
												BtnSellerView,
												DetailView,
												Impression,
												Photoview,
												Response,
												TotalViews,
												EventSource                      
										)
										SELECT  LogDate,
												ProfileId,
												BtnSellerView,
												DetailView,
												Impression,
												Photoview,
												Response,
												TotalViews,
												eventsource
										FROM  IntermediateDealerUsedCarViews  WITH (NOLOCK)
		END ;
         MERGE DealerUsedCarViews  AS SK
							USING (  SELECT * FROM   
											IntermediateDealerUsedCarViews WITH (NOLOCK)
											WHERE LTRIM(RTRIM(ProfileId)) LIKE 'D%'
										) AS  ISK
							ON  SK.InquiryID = REPLACE(LTRIM(RTRIM(ISK.ProfileId)),'D','')
							
							WHEN MATCHED AND SK.SellerType=1  THEN UPDATE SET   SK.ViewCount=LTRIM(RTRIM(ISK.TotalViews))+SK.ViewCount,
															                    SK.LastUpdated=GETDATE()   --SUBSTRING(RTRIM(LTRIM(ISK.LogDate)),1,10)
															
		     				WHEN NOT MATCHED  BY TARGET  THEN
	  									INSERT (InquiryID,
												Sellertype,
												Viewcount,
												LastUpdated)
										VALUES (REPLACE(LTRIM(RTRIM(ISK.ProfileId)),'D',''),
												1,
												LTRIM(RTRIM(ISK.TotalViews)),
												GETDATE()  --SUBSTRING(RTRIM(LTRIM(ISK.LogDate)),1,10)
												);
												
                DROP TABLE #AllFileNames;												
												
         END TRY
		 BEGIN CATCH
		 INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Update Dealer Used Car Views Job',
									        'dbo.UpdateDealerUsedCarViewsJob',
											 ERROR_MESSAGE(),
											 'DealerUsedCarViews',
											 NULL,
											 GETDATE()
                                            )
			END CATCH;
		 
	 END 

