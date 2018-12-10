IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_ActivateBroker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_ActivateBroker]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 29-May-14
-- Description:	To Activate/Deactivate broker From Opr
-- =============================================
CREATE PROCEDURE [dbo].[BA_ActivateBroker] 
	@Mobile VARCHAR(50),
	@RegBrokerId BIGINT,
	@Pin INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(100) = NULL
	
   BEGIN
   --Check if Broker Data is verified or not
   IF (SELECT BRB.IsVerified FROM BA_RegisterBroker AS  BRB WITH (NOLOCK) WHERE BRB.Id = @RegBrokerId) = 1 --Yes
   BEGIN
		IF (SELECT BRB.IsActive FROM BA_RegisterBroker AS  BRB WITH (NOLOCK) WHERE BRB.Id = @RegBrokerId) = 0 --
				BEGIN
					UPDATE [dbo].[BA_RegisterBroker] SET IsActive = 1,DateofActivation = GETDATE()  WHERE ID = @RegBrokerId
						IF (SELECT COUNT(ID) FROM BA_Login WITH (NOLOCk) WHERE BrokerID = @RegBrokerId) = 0 --check if exist in Login table
							INSERT INTO [dbo].[BA_Login] (IsActive, Password,UserName,IsFirstTime,IsVersionUpdated,BrokerID ) VALUES ('1',@Pin,@Mobile,'1','1',@RegBrokerId) 
						ELSE 
							UPDATE BA_Login SET IsActive = 1  WHERE BrokerID = @RegBrokerId

						SET @Message = 'Broker is Successfully Activated' ;
				END
		ELSE 
				SET @Message = 'Broker is Already Active' ;

	SELECT @Pin = Password FROM [dbo].[BA_Login] WITH (NOLOCK) WHERE IsActive = 1 AND BrokerID = @RegBrokerId--get Pin
	END	 
 ELSE
		
	BEGIN
		SET @Message = 'Broker is not Verified.' ;
	END
END	

SELECT @Message AS Message, @Pin AS Pin
	
END
