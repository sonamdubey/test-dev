IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Support_ShowAssignedTicketLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Support_ShowAssignedTicketLog]
GO

	-- =============================================
-- Author:		<Amit Kumar>
-- Create date: <18th may 2013>
-- Description:	<To Save the ticket generated from user>
-- =============================================
CREATE PROCEDURE [dbo].[Support_ShowAssignedTicketLog]
@category		NUMERIC(18,0),
@status			VARCHAR(100),
@frmDate		DATETIME,
@toDate			DATETIME,
@strt 			INT,
@end			INT
AS
BEGIN
	SET NOCOUNT ON;
		WITH CTE AS (
			SELECT  OU1.UserName AS TicketOwner,OU.UserName AS InitiatedBy,ST.LastActionDate AS LastActionDate,SC.Name AS Category,
					ST.Comment AS Problem,ST.TicketDate AS TktGeneretedOn,SSC.Name AS SubCategory,STS.Status AS CurrentStatus
					--CASE ST.Status WHEN '1' THEN 'Open' WHEN '2' THEN 'In Progress' WHEN '3' THEN 'Resloved, Close Now' WHEN '4' THEN 'Reject' WHEN '5' THEN 'Resolved' END  AS CurrentStatus 
					,ST.Id AS TicketId,ISNULL(ST.Rating,'') AS Rating,ISNULL(ST.LastComment,'') AS LastComment,ISNULL(CONVERT(varchar(30),ST.ClosingDate,100),'') AS TktClosedOn
			FROM Support_Tickets ST (NOLOCK)
					INNER JOIN Support_SubCategory SSC (NOLOCK) ON SSC.Id= ST.SubcategoryId
					INNER JOIN Support_Category SC (NOLOCK) ON SC.Id = SSC.CategoryId
					INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = ST.InitiatedBy
					INNER JOIN OprUsers OU1 (NOLOCK) ON OU1.Id = ST.TicketOwner
					INNER JOIN Support_TicketStatus STS (NOLOCK) ON STS.Id=ST.Status
					JOIN dbo.Fnsplitcsvvalueswithidentity(@status) AS MDL ON MDL.ListMember = STS.Id
			WHERE (@category IS NULL OR SC.Id=@category) AND ST.TicketDate BETWEEN @frmDate AND @toDate --ORDER BY ST.LastActionDate DESC
			) 
			SELECT *,ROW_NUMBER()Over (Order by LastActionDate DESC) AS RowId INTO #MyTemp FROM CTE;
			
			SELECT * FROM #MyTemp WHERE RowId BETWEEN @strt AND @end
			
            SELECT COUNT(RowId) AS TotalCount FROM #MyTemp
            
            DROP TABLE #MyTemp
END
