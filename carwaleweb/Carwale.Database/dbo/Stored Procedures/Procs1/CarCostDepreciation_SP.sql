IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarCostDepreciation_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarCostDepreciation_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 20-May-09
-- Description:	SP to insert/update depreciation value
-- =============================================
CREATE PROCEDURE [dbo].[CarCostDepreciation_SP]
	-- Add the parameters for the stored procedure here
	@Id				NUMERIC,
	@MakeId			INT,
	@BodyStyleId	INT,
	@SubSegmentId	INT,
	@FuelType		VARCHAR(50),
	@DepYr1			Decimal(5,2),
	@DepYr2			Decimal(5,2),
	@DepYr3			Decimal(5,2),
	@DepYr4			Decimal(5,2),
	@DepYr5			Decimal(5,2),
	@Status			SMALLINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF NOT EXISTS( SELECT Id FROM CarCostDepreciation WHERE MakeId = @MakeId AND BodyStyleId = @BodyStyleId AND SubSegmentId = @SubSegmentId )
		BEGIN
			IF @Id <> -1	
				BEGIN
					-- UPDATE New values with old values
					UPDATE CarCostDepreciation 
					SET MakeId = @MakeId, BodyStyleId = @BodyStyleId, FuelType = @FuelType,
						SubSegmentId = @SubSegmentId,
						DepYear1 = @DepYr1, DepYear2 = @DepYr2,
						DepYear3 = @DepYr3, DepYear4 = @DepYr4, DepYear5 = @DepYr5
					WHERE Id = @Id

					SET @Status = 1
				END
			ELSE
				BEGIN
					INSERT INTO CarCostDepreciation(MakeId, BodyStyleId, SubSegmentId, FuelType, 
						DepYear1, DepYear2, DepYear3, DepYear4, DepYear5)
					VALUES(@MakeId, @BodyStyleId, @SubSegmentId, @FuelType, 
						@DepYr1, @DepYr2, @DepYr3, @DepYr4, @DepYr5 )

					SET @Status = 2
				END 
		END  
END