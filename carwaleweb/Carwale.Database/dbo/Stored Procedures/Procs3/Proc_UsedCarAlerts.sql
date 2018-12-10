IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Proc_UsedCarAlerts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Proc_UsedCarAlerts]
GO

	
CREATE PROCEDURE [dbo].[Proc_UsedCarAlerts] 
	@CustomerId		AS Numeric, 
	@CityId			AS Numeric, 
	@CityDistance		AS Numeric, 
	@BodyStyleId		AS Numeric, 
	@SegmentId		AS Numeric, 
	@MakeId		AS VarChar(3000), 
	@ModelId		AS VarChar(3000), 
	@VersionId		AS VarChar(3000), 
	@FromYear		AS Numeric, 
	@ToYear		AS Numeric, 
	@FromPrice		AS Numeric, 
	@ToPrice		AS Numeric, 
	@FromKm		AS Numeric, 
	@ToKm			AS Numeric, 
	@SellerType		AS Numeric, 
	@EntryDateTime	AS DateTime, 
	@EntryDateTicks	AS Char(18),
	@RecordId 		AS Numeric Output

AS
BEGIN
	INSERT INTO UsedCarAlerts
		(
			CustomerId, 	CityId, 			CityDistance, 		BodyStyleId, 		SegmentId, 
			MakeId, 	ModelId, 		VersionId, 		FromYear, 		ToYear, 
			FromPrice, 	ToPrice, 		FromKm, 		ToKm, 			SellerType, 
			IsActive, 	EntryDateTime, 		EntryDateTicks
		)
	VALUES
		(
			@CustomerId, 	@CityId, 		@CityDistance, 		@BodyStyleId, 		@SegmentId, 
			@MakeId, 	@ModelId, 		@VersionId, 		@FromYear, 		@ToYear, 
			@FromPrice, 	@ToPrice, 		@FromKm, 		@ToKm, 		@SellerType, 
			1, 		@EntryDateTime, 	@EntryDateTicks
		)

	SET @RecordId = SCOPE_IDENTITY()

	print @RecordId
END