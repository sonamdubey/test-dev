IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Support_ShowTicketLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Support_ShowTicketLog]
GO

	-- =============================================
-- Author:		<Amit Kumar>
-- Create date: <18th may 2013>
-- Description:	<To Save the ticket generated from user>
-- =============================================
CREATE PROCEDURE [dbo].[Support_ShowTicketLog]
@oprUserId		NUMERIC(18,0),
@status			VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT  OU1.UserName AS TicketOwner,OU.UserName AS InitiatedBy,ST.LastActionDate AS LastActionDate,SC.Name AS Category,
						ST.Comment AS Problem,ST.TicketDate AS TktGeneretedOn,SSC.Name AS SubCategory,STS.Status AS CurrentStatus
						--CASE ST.Status WHEN '1' THEN 'Open' WHEN '2' THEN 'In Progress' WHEN '3' THEN 'Resloved, Close Now' WHEN '4' THEN 'Reject' WHEN '5' THEN 'Resolved' END AS CurrentStatus 
						,ST.Id AS TicketId,ISNULL(ST.Rating,'') AS Rating,ISNULL(ST.LastComment,'') AS LastComment,ISNULL(CONVERT(varchar(30),ST.ClosingDate,100),'') AS TktClosedOn
				FROM Support_Tickets ST (NOLOCK)
						INNER JOIN Support_SubCategory SSC (NOLOCK) ON SSC.Id= ST.SubcategoryId
						INNER JOIN Support_Category SC (NOLOCK) ON SC.Id = SSC.CategoryId
						INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = ST.InitiatedBy
						INNER JOIN OprUsers OU1 (NOLOCK) ON OU1.Id = ST.TicketOwner
						INNER JOIN Support_TicketStatus STS (NOLOCK) ON STS.Id=ST.Status
					    JOIN dbo.Fnsplitcsvvalueswithidentity(@status) AS MDL ON MDL.ListMember = STS.Id
						WHERE (ST.InitiatedBy=@oprUserId OR ST.TicketOwner=@oprUserId) --AND (@status IS NULL OR ST.Status IN (@status))
				ORDER By ST.TicketDate DESC
END
