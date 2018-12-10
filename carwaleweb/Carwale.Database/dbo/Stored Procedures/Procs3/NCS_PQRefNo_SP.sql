IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_PQRefNo_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_PQRefNo_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 16-Oct-08 5:48 PM
-- Description:	SP to save final price quote of module NCS(New Car Sales)
-- =============================================
CREATE PROCEDURE [dbo].[NCS_PQRefNo_SP] 
	-- Add the parameters for the stored procedure here
	
	@UserId				NUMERIC,
	@SourceId			SmallInt, -- 1 from Opr and 2 from ICICI and so on....
	@EntryDateTime		DateTime,
	@BuyTime			Varchar(50),
	@VersionId			Int,
	@CityId				Int,
	
	-- Output Parameter
	@PQRefNo		VARCHAR(50) Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @RefId Numeric, @RefDate VarChar(50), @SourceCode VarChar(50)

	Insert Into NCS_PQRefNo
	(
		UserId, EntryDateTime, SourceId, BuyTime, VersionId, CityId
	)
	Values
	(
		@UserId, @EntryDateTime, @SourceId, @BuyTime, @VersionId, @CityId
	)

	Set @RefId = Scope_Identity()
	Set @RefDate = Convert( Varchar, GetDate(), 112)

	IF @SourceId = 1 SET @SourceCode = 'CW'
	ELSE IF @SourceId = 2 SET @SourceCode = 'IC'

	Set @PQRefNo = @SourceCode + Substring(@RefDate, 3, Len(@RefDate)) + Convert(VarChar, @RefId)
	
	UPDATE NCS_PQRefNo Set PQRefNo = @PQRefNo WHERE Id = @RefId
	
END












