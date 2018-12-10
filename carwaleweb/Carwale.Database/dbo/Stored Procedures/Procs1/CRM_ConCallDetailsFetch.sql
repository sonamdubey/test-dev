IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ConCallDetailsFetch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ConCallDetailsFetch]
GO

	-- =============================================
-- Author:		<Amit Kumar>
-- Create date: <26 Nov 2012>
-- Description:	<Getting Con Call Log>
-- =============================================
CREATE PROCEDURE [dbo].[CRM_ConCallDetailsFetch] 
@cbdId				NUMERIC(18,0)
AS
BEGIN
	--UPDATE CRM_ConCall SET ConCallValue = @ConCallValue WHERE CBDId = @cbdId
	 
	SELECT CCC.ConCallValue,CCC.Id FROM CRM_ConCall CCC WITH(NOLOCK) WHERE CBDId = @cbdId
END
