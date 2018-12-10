IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_RemoveContactFromUseGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_RemoveContactFromUseGroup]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_RemoveContactFromUseGroup] 
	@GroupId INT,
	@DealerIds VARCHAR(200),		---|Combination of DealerId And IsCarWaleDealers= >Primary Key 
	@IsCarWaleDealers VARCHAR(200) ----|
AS
BEGIN
	SET NOCOUNT OFF;
	DECLARE @Status BIT = 0

	

	UPDATE BA_GroupDetails SET IsActive = 0 
						  WHERE ID IN (SELECT BGD.ID FROM BA_GroupDetails AS BGD WITH (NOLOCK) 
							INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@DealerIds) AS D ON BGD.DealerId = D.ListMember
							INNER JOIN   [dbo].fnSplitCSVValuesWithIdentity(@IsCarWaleDealers) AS ICW ON ICW.ListMember = BGD.IsCarWaleDealer AND ICW.id = D.id)

			--WHERE DealerId IN (select ListMember from [dbo].fnSplitCSV(@DealerIds))
			--AND IsCarWaleDealer IN  (select IC.ListMember from [dbo].fnSplitCSV(@IsCarWaleDealers) AS IC) 

SET @Status = @@ROWCOUNT --Set the status value || Success

--Return the Status
SELECT  @Status AS Status

END
