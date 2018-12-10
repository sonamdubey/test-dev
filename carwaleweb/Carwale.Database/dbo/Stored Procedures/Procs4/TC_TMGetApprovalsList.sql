IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetApprovalsList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetApprovalsList]
GO

	-- =============================================
-- Author:		<Author, Nilesh Utture>
-- Create date: <Create Date, 28th Nov, 2013>
-- Description:	<Description, Gives the Approval List to NSC and Regional Manager levels>
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMGetApprovalsList]

	@TC_SpecialUserId INT
AS
BEGIN
	DECLARE @Designation TINYINT

	SELECT  @Designation = Designation FROM TC_SpecialUsers WHERE TC_SpecialUsersId = @TC_SpecialUserId

	IF (@Designation = 3)
	BEGIN
		SELECT  TCM.TC_AMId, TSU1.UserName, TCM.Year  FROM 
			TC_SpecialUsers TSU WITH (NOLOCK) 
			INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) 
				ON TSU.NodeCode =SUBSTRING (TSU1.NodeCode, 1, LEN(TSU.NodeCode))
				AND TSU.Designation=3
				AND TSU1.Designation=4
				AND TSU.TC_SpecialUsersId = @TC_SpecialUserId
			INNER JOIN TC_TMAMTargetChangeMaster TCM
				ON TCM.TC_AMId = TSU1.TC_SpecialUsersId
				AND TCM.IsActive =1
				AND TCM.IsAprrovedByRM IS NULL
			
	END
	ELSE IF (@Designation = 2)
	BEGIN
		SELECT  TCM.TC_AMId, TSU.UserName, TCM.Year FROM
				TC_TMAMTargetChangeMaster TCM  WITH (NOLOCK) 
			INNER JOIN TC_SpecialUsers TSU  WITH (NOLOCK) 
				ON TSU.TC_SpecialUsersId = TCM.TC_AMId
		WHERE	TCM.IsActive = 1
			AND TCM.IsAprrovedByRM = 1	
	END
END
