IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Support_ShowParticularTicketLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Support_ShowParticularTicketLog]
GO

	-- =============================================
-- Author:		<Amit Kumar>
-- Create date: <18th may 2013>
-- Description:	<To Save the ticket generated from user>
-- Modified By : Komal Manjare on 26-09-2016
-- Desc : Get Expected closing date for ticket
-- =============================================
CREATE PROCEDURE [dbo].[Support_ShowParticularTicketLog]
@tktId		NUMERIC(18,0)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT  OU1.UserName AS TicketOwner,OU.UserName AS InitiatedBy,ST.LastActionDate AS LastActionDate,SC.Name AS Category,
						ST.Comment AS Problem,ST.TicketDate AS TktGeneretedOn,SSC.Name AS SubCategory,OU1.LoginId AS OwnerEmailId,OU2.LoginId AS AssigneeEmailId,OU2.UserName AS AssigneeName,STS.Status AS CurrentStatus
						--CASE ST.Status WHEN '1' THEN 'Open' WHEN '2' THEN 'In Process' WHEN '3' THEN 'Resloved, Close Now' WHEN '4' THEN 'Reject' WHEN '5' THEN 'Resolved' END  AS CurrentStatus 
						,ST.Id AS TicketId,ISNULL(ST.Rating,'') AS Rating,ST.ExpecClosingDate -- Komal Manjare on 26-09-2016
				FROM Support_Tickets ST (NOLOCK)
						INNER JOIN Support_SubCategory SSC (NOLOCK) ON SSC.Id= ST.SubcategoryId
						INNER JOIN Support_Category SC (NOLOCK) ON SC.Id = SSC.CategoryId
						INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = ST.InitiatedBy
						INNER JOIN OprUsers OU1 (NOLOCK) ON OU1.Id = ST.TicketOwner
						INNER JOIN OprUsers OU2 (NOLOCK) ON OU2.Id = SSC.AssignedTo
						INNER JOIN Support_TicketStatus STS (NOLOCK) ON STS.Id=ST.Status
				WHERE ST.Id = @tktId
END


