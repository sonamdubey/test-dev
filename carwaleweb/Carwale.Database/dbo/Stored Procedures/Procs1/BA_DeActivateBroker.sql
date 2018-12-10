IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_DeActivateBroker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_DeActivateBroker]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_DeActivateBroker]
	@RegBrokerId BIGINT
	--@Mobile VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(100) = NULL
	-----DeActivate register Broker (present in the table BA_RegisterBroker )	
  IF (SELECT COUNT(BRB.ID) FROM BA_RegisterBroker AS BRB  WITH(Nolock) WHERE BRB.ID = @RegBrokerId) <> 0
			BEGIN
			IF (SELECT BRB.IsActive FROM BA_RegisterBroker AS BRB  WITH(Nolock) WHERE BRB.ID = @RegBrokerId) = 1
				BEGIN
					UPDATE   [dbo].[BA_RegisterBroker] SET IsActive = 0 WHERE ID = @RegBrokerId ;--BA_RegisterBroker
					UPDATE  [dbo].[BA_Login] SET IsActive = 0 WHERE BrokerID = @RegBrokerId ;--Login
					SET @Message = 'Successfully Deactivated.' ;
				END
				ELSE
					SET @Message = 'Already De-Activate.' ;
			END
		
 ELSE
		BEGIN
			SET @Message = 'Broker is not Register' ;
		END

SELECT @Message AS Message ---
END
