IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQActivityFeedLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQActivityFeedLoad]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date, 26th April, 2013>
-- Description:	<Description, Will give activity feed on lead followup page>
-- exec TC_INQActivityFeedLoad 5038
-- Modified By: Umesh Ojha on 26 jul 2013 for fetching activity feed for transfer lead from U1 to U2
-- Modified By: Umesh Ojha on 31 jul 2013 for fetching activity feed for transfer lead from U1 to U2
--              on column Action previously it is fetching in action comment
-- Modified By : Ashwini Dhamankar on Oct 25,2016 (Fetched TC_NextActionId and InquiryId) 
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQActivityFeedLoad] 
	-- Add the parameters for the stored procedure here
	@LeadId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		with Cte as (
				SELECT   U.UserName AS [UserName],
						ISNULL(TCC.ActionComments + ' -','--') AS [ActionComments] ,
						ActionTakenOn AS [ActionTakenOn],
						TCA.Name  AS [Action]
					   ,1 AS [IsComment]
					   ,ActionTakenOn AS [date]
					   ,TCC.NextFollowUpDate AS [FollowUpDate]
					   ,CASE WHEN TCC.TC_NextActionId IS NOT NULL THEN NA.NextAction ELSE '' END AS NextAction -- Modified By : Ashwini Dhamankar on Oct 25,2016
					   ,NULL AS InquiryId
				FROM TC_Calls      AS TCC WITH (NOLOCK)
				JOIN TC_Users      AS U   WITH (NOLOCK)
										 ON TCC.TC_UsersId=U.Id
				LEFT OUTER JOIN TC_CallAction AS TCA WITH (NOLOCK)
										 ON TCA.TC_CallActionId=TCC.TC_CallActionId AND TCA.IsActive=1
				LEFT JOIN TC_NextAction AS NA WITH(NOLOCK) ON NA.TC_NextActionId = TCC.TC_NextActionId
				WHERE   TCC.TC_LeadId=@LeadId
					AND TCC.IsActionTaken=1
				UNION
				SELECT U.UserName AS [UserName],
					   CASE  TCLD.IsClosed WHEN 1 THEN 'Inquiry Closed - '
					   WHEN 0 THEN 
					   (CASE WHEN ISNULL(TCDL.LeadOwnerId,0) <> 0 AND ISNULL(TCDL.NewLeadOwnerId,0)<> 0 THEN 
					   TCLD.Name ELSE NULL END) END AS [ActionComments],					   					 
					   EventCreatedOn  AS [ActionTakenOn],
					   CASE TCLD.IsClosed WHEN 1 THEN TCLD.Name
					   WHEN 0 THEN
					   (CASE WHEN ISNULL(TCDL.LeadOwnerId,0) <> 0 AND ISNULL(TCDL.NewLeadOwnerId,0)<> 0 THEN 
					   TCLD.Name+' from '  +U1.UserName+' to '+U2.UserName 
					   ELSE CASE WHEN TCDL.TC_LeadDispositionId = 88 THEN TCLD.Name + ' - '+ ISNULL(TCDL.DispositionReason,'') ELSE TCLD.Name END END) END AS [Action],					    					     
					   0 AS [IsComment],
					   EventCreatedOn AS [date],
					   NULL AS [FollowUpDate],
					   '' AS NextAction
					   ,TCDL.InqOrLeadId AS InquiryId
				FROM TC_DispositionLog AS TCDL WITH (NOLOCK) -- Removed VWAllTC_DispositionLog by Deepak on 11th July
				JOIN TC_Users AS U WITH (NOLOCK) ON TCDL.EventOwnerId=U.Id
				Left JOIN TC_Users AS U1 WITH (NOLOCK) ON TCDL.LeadOwnerId = U1.Id
				Left JOIN TC_Users AS U2 WITH (NOLOCK) ON TCDL.NewLeadOwnerId = U2.Id
				JOIN TC_LeadDisposition AS TCLD WITH (NOLOCK) ON TCLD.TC_LeadDispositionId=TCDL.TC_LeadDispositionId
				WHERE TCDL.TC_LeadId=@LeadId
				)
				SELECT [UserName],[ActionComments],[ActionTakenOn],[Action],[IsComment],[FollowUpDate],[NextAction],[InquiryId] FROM Cte ORDER BY [date] DESC;
END

-----------------
