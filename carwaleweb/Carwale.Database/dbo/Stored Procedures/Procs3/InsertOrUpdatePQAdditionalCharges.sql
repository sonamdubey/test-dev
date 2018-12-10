IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertOrUpdatePQAdditionalCharges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertOrUpdatePQAdditionalCharges]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 21/06/2016
-- Description:	To save and update additional charges 
-- Modified : Vicky Lund, 22/07/2016, Added Explanation column
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrUpdatePQAdditionalCharges]
	-- Add the parameters for the stored procedure here
	@Id INT
	,@CategoryName VARCHAR(50)
	,@Type INT
	,@Scope INT
	,@Explanation varchar(2000)
	,@UpdatedBy INT
	,@NewId INT = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @Id = 0
	BEGIN
		INSERT INTO PQ_CategoryItems (
			CategoryId
			,CategoryName
			,[Type]
			,Scope
			,Explanation
			,UpdatedBy
			,UpdatedOn
			,IsActive
			)
		VALUES (
			CASE 
				WHEN @Type = 1
					THEN 6 -- compulsory charges
				WHEN @Type = 2
					THEN 7 -- optional charges
				END
			,@CategoryName
			,@Type
			,@Scope
			,@Explanation
			,@UpdatedBy
			,GETDATE()
			,1
			)

		SET @NewId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE PQ_CategoryItems
		SET CategoryName = @CategoryName
			,[Type] = @Type
			,Scope = @Scope
			,Explanation = @Explanation
			,CategoryId = CASE 
				WHEN @Type = 1
					THEN 6 -- compulsory charges
				WHEN @Type = 2
					THEN 7 -- optional charges
				END
			,UpdatedBy = @UpdatedBy
			,UpdatedOn = GETDATE()
		WHERE Id = @Id

		SET @NewId = @Id
	END
END

