IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CustomerControlSearch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CustomerControlSearch]
GO

	
-- =============================================
-- New SP
-- Author:		<Ashwini Dhamankar>
-- Create date: <11/08/2014>
-- Description:	<To get open and closed lead records>
--exec [TC_CustomerControlSearch] 5,1,1,2000,'9989394969',NULL,1
--MOdified By: Tejashree Patil on 27 Aug 2014, Changed join conditions.
-- =============================================
CREATE PROCEDURE [dbo].[TC_CustomerControlSearch]

	-- Add the parameters for the stored procedure here
	@BranchId        BIGINT, 
	@UserId          BIGINT, 
	@FromIndex       INT, 
	@ToIndex         INT, 
	@CustomerMobile  VARCHAR(50), 
	@CustomerEmail   VARCHAR(100),
	@SearchText      VARCHAR(50) = NULL
		
AS
BEGIN
	IF (@SearchText IS NULL)
	BEGIN 
	SET @UserId=NULL
	END ;
    WITH Cte1 
           AS (SELECT C.id                  AS [CustomerId], 
                      (ISNULL(C.Salutation,'')+' '+ C.CustomerName+' '+ISNULL(C.LastName,''))        AS [CustomerName], 
                      C.Email,    
                      C.Mobile,  
                      C.TC_InquirySourceId,                 
                      TCIL.TC_LeadId, 
                      TC_InquiryStatusId, 
                      ScheduledOn           AS [NextFollowUpDate], 
                      TCIL.CarDetails       AS [InterestedIn], 
                      TCAC.CallType,
                      TCAC.LastCallComment,
                      LatestInquiryDate,
                      (CASE  WHEN LatestInquiryDate > ScheduledOn THEN LatestInquiryDate ELSE ScheduledOn END) AS OrderDate,
                      TS.Source AS InquirySource,
                      TCIL.TC_UserId AS UserId,
                      TCIL.TC_LeadStageId,
                      TCIL.TC_LeadDispositionID,
                      UniqueCustomerId,
					  TCAC.TC_NextActionId,
                      CASE TCIL.TC_LeadInquiryTypeId WHEN 1 THEN 'Used Buy' 
												WHEN 2 THEN 'Used Sell' 
												WHEN 3 THEN 'New Buy' END AS InquiryType  --Commented by Manish on 16-07-2013 
                    
               FROM           
                             /* TC_ActiveCalls         AS TCAC  WITH (NOLOCK) 
                      JOIN    TC_CustomerDetails     AS C     WITH (NOLOCK) 
                                                                           ON TCAC.TC_LeadId = C.ActiveLeadId 
                      JOIN    TC_InquiriesLead       AS TCIL  WITH (NOLOCK) 
                                                                           ON TCAC.TC_LeadId = TCIL.TC_LeadId 
                      JOIN    TC_Users               AS TU    WITH(NOLOCK) 
						                                                   ON TCIL.TC_UserId = TU.Id	
					  JOIN    TC_InquirySource       AS TS 	  WITH(NOLOCK) 
						                                                   ON C.TC_InquirySourceId = TS.Id	*/
																		   
						--Modified By: Tejashree Patil on 27 Aug 2014, Commented above joins.
										TC_CustomerDetails     AS C		WITH (NOLOCK) 
						INNER	JOIN    TC_InquiriesLead       AS TCIL  WITH (NOLOCK) ON C.id= TCIL.TC_CustomerId
						INNER	JOIN    TC_InquirySource       AS TS 	WITH (NOLOCK) ON C.TC_InquirySourceId = TS.Id
						INNER	JOIN    TC_Users               AS TU    WITH(NOLOCK)  ON TCIL.TC_UserId = TU.Id		    
						LEFT	JOIN    TC_ActiveCalls         AS TCAC  WITH (NOLOCK) ON TCAC.TC_LeadId = TCIL.TC_LeadId 
               WHERE  
						 ( TCAC.TC_UsersId = @UserId  OR  @UserId  IS NULL)
                AND      (TCIL.TC_UserId=@UserId OR  @UserId  IS NULL)
                AND      TCIL.BranchId=@BranchId
                AND      ( ( @CustomerMobile IS NULL )       OR ( C.Mobile LIKE @CustomerMobile +'%' ) ) 
                AND      ( ( @CustomerEmail IS NULL )        OR ( C.Email LIKE @CustomerEmail + '%') )
                ), 
           Cte2 
           AS (SELECT *, 
                      ROW_NUMBER() 
                        OVER (                                                ---line commented and chages made by manish on 16-07-2013
                               PARTITION BY TC_LeadId ORDER BY OrderDate DESC    -- ORDER BY  NextFollowUpDate DESC, LatestInquiryDate DESC
                         ) RowNumber
               FROM   Cte1
               )
         
              SELECT *, ROW_NUMBER() OVER (ORDER BY OrderDate DESC) NumberForPaging
              INTO   #TblTemp 
              FROM   Cte2 
              WHERE  RowNumber = 1
                
		      SELECT * 
		      FROM   #TblTemp 
		      WHERE  
			  (@FromIndex IS NULL AND @ToIndex IS NULL)	  OR  (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
		   
			  SELECT COUNT(*) AS RecordCount FROM #TblTemp
			 
		      DROP TABLE #TblTemp 
END

