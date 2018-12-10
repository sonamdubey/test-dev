IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_HeadBranchLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_HeadBranchLeads]
GO

	
-- =============================================
-- Author:		<Author : Vinay Kumar Prajapati>
-- Create date: <21/02/2014>
-- Description:	<This SP Gives the report for HeadBranch lead summary for given date range.
-- =============================================
CREATE PROCEDURE [dbo].[NCD_HeadBranchLeads]
	-- Add the parameters for the stored procedure here
(
 @StartDate DATETIME,
 @EndDate DATETIME
)
AS
BEGIN
	 SELECT  NI.DealerId,DNC.Name AS Dealer, COUNT(NI.ID) AS Total , NI.IsAccepted  
     FROM NCD_Inquiries AS NI WITH(NOLOCK)
		 INNER JOIN NCD_HeadBranchDealers AS NHB WITH(NOLOCK) ON  NHB.DealerId=NI.DealerId AND NI.IsActionTaken=1
		 LEFT JOIN Dealer_NewCar AS DNC WITH(NOLOCK) ON DNC.Id=NHB.DealerId
	 WHERE NI.EntryDate BETWEEN @StartDate AND @EndDate
     GROUP BY  NI.IsAccepted ,NI.DealerId,DNC.Name
 
END