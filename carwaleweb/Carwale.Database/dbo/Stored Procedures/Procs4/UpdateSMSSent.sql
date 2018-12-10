IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateSMSSent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateSMSSent]
GO

	-- =============================================
-- Author:		Skhikhar 
-- Create date: 26-03-2012
-- Description:	To update the SMSSent table with service provider data
-- =============================================
CREATE PROCEDURE [dbo].[UpdateSMSSent]
	@Id			NUMERIC,
	@returnMsg	VARCHAR(500),
	@Provider	VARCHAR(250),
	@IsSMSSent	BIT
AS
BEGIN
	UPDATE dbo.SMSSent
	SET 
		ServiceProvider =	@Provider,
		ReturnedMsg		=	@returnMsg,
		SMSSentDateTime =	GETDATE(),
		IsSMSSent		=	@IsSMSSent
	WHERE 
		ID = @Id
END