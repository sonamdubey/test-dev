IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertOrUpdatePriceAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertOrUpdatePriceAvailability]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 27/06/2016
-- Description:	To save or update price availability
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrUpdatePriceAvailability]
	-- Add the parameters for the stored procedure here
	@Id INT
	,@RuleName VARCHAR(50)
	,@Explanation VARCHAR(100)
	,@Type INT
	,@UpdatedBy INT
	,@NewId INT = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @Id = 0
	BEGIN
		INSERT INTO PQ_PriceAvailability (
			NAME
			,Explanation
			,Type
			,UpdatedBy
			,UpdatedOn
			,IsActive
			)
		VALUES (
			@RuleName
			,@Explanation
			,@Type
			,@UpdatedBy
			,GETDATE()
			,1
			)

		SET @NewId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE PQ_PriceAvailability
		SET NAME = @RuleName
			,Explanation = @Explanation
			,[Type] = @Type
			,UpdatedBy = @UpdatedBy
			,UpdatedOn = GETDATE()
		WHERE Id = @Id

		SET @NewId = @Id
	END
END

