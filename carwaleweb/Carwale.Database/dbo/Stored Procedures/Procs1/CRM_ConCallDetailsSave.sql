IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ConCallDetailsSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ConCallDetailsSave]
GO

	-- =============================================
-- Author:		<Amit Kumar>
-- Create date: <26 Nov 2012>
-- Description:	<Saving Con Call Log>
-- =============================================
CREATE PROCEDURE [dbo].[CRM_ConCallDetailsSave] 
@cbdId				NUMERIC(18,0),
@updatedBy			NUMERIC(18,0),
@updatedOn			DATETIME,
@ConCallValue		BIGINT

AS
BEGIN
	UPDATE CRM_ConCall SET UpdatedBy= @updatedBy, UpdatedOn = @updatedOn, ConCallValue = @ConCallValue WHERE CBDId = @cbdId
	IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CRM_ConCall(CBDId,UpdatedBy,UpdatedOn,ConCallValue) VALUES (@cbdId,@updatedBy,@updatedOn,@ConCallValue)
		END

END
