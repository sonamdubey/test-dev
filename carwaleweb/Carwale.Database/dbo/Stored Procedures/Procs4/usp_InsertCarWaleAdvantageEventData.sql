IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[usp_InsertCarWaleAdvantageEventData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[usp_InsertCarWaleAdvantageEventData]
GO

	 
-- =============================================
-- Author:		Kundan Dombale
-- Create date: 11-03-201g=6
-- Description:	Load the data from text file for dealers car view count and update into [InsertCarWaleAdvantageEventData]
 
-- =============================================
CREATE PROCEDURE [dbo].[usp_InsertCarWaleAdvantageEventData]
 AS 
 SET NOCOUNT ON;
    BEGIN
		
		
		
		
		BEGIN TRY
	    TRUNCATE  TABLE IntermediateCarWaleAdvantageEventData;

	   CREATE TABLE #AllFileNames ( ID INT IDENTITY(1,1) 
	                               ,FileName varchar(150)
								  );

       DECLARE @FileName VARCHAR(255),
               @FilePath     VARCHAR(255)='Z:\CarWaleAdvantage\',
               @MoveFilePath     VARCHAR(255)='Z:\ProcessedCarWaleAdvantage\',
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
			
			SET @SQL='BULK INSERT [IntermediateCarWaleAdvantageEventData] FROM  '  +''''+@FilePath + @FileName+''''+
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
						            VALUES ('InsertCarWaleAdvantageEventData',
									        'InsertCarWaleAdvantageEventData',
											 ERROR_MESSAGE(),
											 'InsertCarWaleAdvantageEventData',
											 'FileName:'+ @FileName,
											 GETDATE()
                                            )
			 
			 END CATCH 
			 
			 SET @WhileLoopControl=@WhileLoopControl+1;
			 
	    END ;
		
		BEGIN								--- Added  Block to maintian the logs of IntermediateDealerUsedCarViews table 
			INSERT INTO CarWaleAdvantageEventData (
													stockid   ,
													EventDate  ,
													Cityid   ,
													CategoryId  ,
													PlatformId  ,
													[Counter]                       
																								)
										     SELECT  
													CONVERT(BIGINT,stockid )as stockid  ,
													SUBSTRING(RTRIM(LTRIM(EventDate)),1,10) as EventDate ,
													CONVERT(int,Cityid)as Cityid   ,
													CONVERT(int,CategoryId)as CategoryId  ,
													CONVERT(INT,PlatformId) as PlatformId ,
													CONVERT(BIGINT,[Counter]) as [Counter]   
										FROM  [dbo].[IntermediateCarWaleAdvantageEventData]  WITH (NOLOCK)
		END ;

          
												
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
						            VALUES ('Update CarWaleAdvantageS tock Views',
									        'UpdateCarWaleAdvantageStockViews',
											 ERROR_MESSAGE(),
											 'CarWaleAdvantageStockViews',
											 'FileName:'+ @FileName,
											 GETDATE()
                                            )
			END CATCH;

		 

	 END 