IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ComputeLLSortScoreHourlyExecution]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ComputeLLSortScoreHourlyExecution]
GO

	-- =============================================
-- Author:		Purohith Guguloth	
-- Create date: 12th December, 2015
-- Description:	Computes the SortScore for Stock of Dealers and 
--              Updates Livelistings table .
-- =============================================
CREATE PROCEDURE [dbo].[ComputeLLSortScoreHourlyExecution]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @NumRecordsToUpdateOnce INT = 100,
	        @WhileLoopCount INT,
	        @WhileLoopControl INT = 0,
	        @NumRecordsUpdated INT;

    -- Creating a temp table to store DealerId's of all the dealers who got responses in the last one hour 
	--Select * INTO #tempDealers from
	--(Select distinct LL.DealerId 
	--    from UsedCarPurchaseInquiries UCP WITH(NOLOCK) 
	--    INNER JOIN LiveListings LL WITH(NOLOCK) on UCP.SellInquiryId = LL.Inquiryid AND LL.SellerType = 1
	--    where UCP.RequestDateTime > DATEADD(minute, -60, GETDATE())
 --   UNION 
	--Select distinct LL.DealerId
	--	from MM_Inquiries MMI WITH(NOLOCK)
	--	INNER JOIN LiveListings LL WITH(NOLOCK) on MMI.ConsumerId = LL.DealerId AND LL.SellerType = 1 AND MMI.CallStatus = 'success'
	--	where MMI.CallEndDate > DATEADD(minute, -60, GETDATE())) as temp
	    
	--Removing the Existing entries from the table to insert the current records 
	--TRUNCATE table LLFinalSortScore      
	    	    
	-- Creating a temp table with the updated SortScore 
	Select IDENTITY (INT, 1, 1) AS ID,a.Inquiryid,a.Score,a.SellerType Into #tempLLFinalSortScore 
	From(
			SELECT (
				(CASE WHEN PT.Priority in (1,2) THEN 
					CASE WHEN LL.responses > 15 THEN 2 - (CAST(LL.Responses AS FLOAT)/10000) + (CASE WHEN PSS.NewScore < 0 THEN (PSS.NewScore/100) ELSE PSS.NewScore END / 10000)
					ELSE 		
						--3 + ( CASE WHEN TLL.Score < 0 THEN (TLL.Score/100) ELSE TLL.Score END ) - (CAST(TLL.Responses AS FLOAT)/10000000000)
						CASE WHEN PSS.SVScore<0 THEN
							(
								4 + ( CASE WHEN PSS.NewScore < 0 THEN (PSS.NewScore/100) ELSE PSS.NewScore END ) - (CAST(LL.Responses AS FLOAT)/10000000000)
							)
							ELSE
							(
								3 + ( CASE WHEN PSS.NewScore < 0 THEN (PSS.NewScore/100) ELSE PSS.NewScore END ) - (CAST(LL.Responses AS FLOAT)/10000000000)
							)
						END
					END
				ELSE 1 + PSS.NewScore END ) 
			    + CASE WHEN LL.PhotoCount > 0 THEN 0 ELSE -5 END
			) 
			As Score,LL.Inquiryid,LL.SellerType
		   --INTO #tempLiveListings 
		   FROM livelistings AS LL WITH(NOLOCK)
		   INNER JOIN PaidSellerScore PSS WITH(NOLOCK) ON LL.Inquiryid = PSS.Inquiryid AND LL.SellerType = PSS.SellerType
		   LEFT JOIN PackageTypePriority PT WITH (NOLOCK) ON LL.PackageType = PT.PackageType
		   WHERE LL.SellerType=1
	   ) As a
	   
	--Droping temparary tables
		--Drop table #tempDealers;
	  
    -- Calicualting no of records that needs to update to set @WhileLoopCount
    SELECT @whileLoopCount = COUNT(1) FROM #tempLLFinalSortScore WITH(NOLOCK)
    
    -- Looping through LLFinalSortScore to Update Livelistings table 25 Records at a time
    While (@WhileLoopCount > @WhileLoopControl                   
         OR @WhileLoopCount =@WhileLoopControl-@NumRecordsToUpdateOnce+(@WhileLoopCount%@NumRecordsToUpdateOnce))      
		BEGIN     
			BEGIN TRY         
				UPDATE ll        
					SET ll.SortScore = LFS.Score                 
					FROM livelistings ll WITH(NOLOCK)       
					INNER JOIN #tempLLFinalSortScore LFS WITH(NOLOCK)       
					ON LFS.Inquiryid = ll.Inquiryid AND        
					LFS.SellerType = ll.SellerType     
				WHERE LFS.ID BETWEEN @WhileLoopControl+1 AND (@WhileLoopControl + @NumRecordsToUpdateOnce);          
			    
				--Update #tempLLFinalSortScore         
				--	SET IsSync = 1     
				--WHERE ID BETWEEN @WhileLoopControl+1 AND (@WhileLoopControl + @NumRecordsToUpdateOnce);   
			END TRY 
		
		BEGIN CATCH
					INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('LiveListing Update SortScore Hourly Execution',
									        'dbo.ComputeLLSortScoreHourlyExecution',
											 ERROR_MESSAGE(),
											 'livelistings',
											 'Failed in the Loop: '+@WhileLoopControl,
											 GETDATE()
                                            )
		END CATCH 
						
		SET @WhileLoopControl=@WhileLoopControl + @NumRecordsToUpdateOnce;
    END
        
END