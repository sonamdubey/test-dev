IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RemoveBlockedNumber]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RemoveBlockedNumber]
GO

	-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <22/09/2016>
-- Description:	<Remove existing blocked number>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveBlockedNumber] @Ids VARCHAR(100)
AS
BEGIN
	DELETE
	FROM BlockedNumbers
	WHERE Id IN (
			SELECT ListMember
			FROM dbo.fnSplitCSVMAx(@Ids)
			)
END

