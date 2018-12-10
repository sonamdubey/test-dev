IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveGuessFeature]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveGuessFeature]
GO

	
-- =============================================
-- Author:		Mihir.A.Chheda
-- Create date: 21-11-2013
-- Description:	add details of customers for Guess The Feature App.
-- =============================================

CREATE PROCEDURE [dbo].[OLM_SaveGuessFeature]
    @Name VARCHAR(50),
	@Email VARCHAR(50),
	@Contact VARCHAR(50),
	@Id Numeric(18,0) OUTPUT
AS

BEGIN

	SET NOCOUNT ON;

	INSERT INTO OLM_GuessFeatureTransaction 
	(Name,Email,Contact) 
	VALUES(@Name,@Email,@Contact)

	SET @Id=SCOPE_IDENTITY()

END

