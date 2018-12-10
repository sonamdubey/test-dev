IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_CheckORCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_CheckORCars]
GO

	
-----Modified By: Manish on 22-04-2014 added WITH(NOLOCK) keyword wherever not found.
CREATE PROCEDURE [dbo].[DCRM_CheckORCars]
	@Id				Numeric = NULL,
	@Type			SmallInt = NULL,  -- 1 for KM out of Range, 2- Price Out of Range
	@InquiryId		Numeric,
	@km				Numeric = NULL,
	@Price			Numeric = NULL,
	@MakeYear		DateTime = NULL,
	@UpdatedBy		Numeric = NULL
	
 AS
	DECLARE @ValuationPrice AS Numeric, @Year AS Numeric
	
BEGIN
		
		IF @Id = -1 OR @Id IS NULL
			BEGIN
				--Check Whether KM is out of range or not
				SET @Year = DATEDIFF(YYYY, @MakeYear, GETDATE())
				
				IF @km > @Year * 15000
					BEGIN
						SELECT ID FROM DCRM_CarsORData WITH (NOLOCK) WHERE InquiryId = @InquiryId AND ORKm = 1
						
						IF @@ROWCOUNT = 0
							INSERT INTO DCRM_CarsORData(InquiryId, ORKm) VALUES(@InquiryId, 1)
					END
				ELSE
					BEGIN
						UPDATE DCRM_CarsORData SET ORKm = 0, UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy
						WHERE InquiryId = @InquiryId
					END
					
				--Check Whether Price is out of range or not
				SELECT @ValuationPrice = Valuation FROM BestDealCarValuations WITH (NOLOCK)
				WHERE UserType = 1 AND CarId = @InquiryId
				
				IF @@ROWCOUNT <> 0
					BEGIN
						IF (@Price - @ValuationPrice) > (@ValuationPrice * 20)/100 
							BEGIN
								SELECT ID FROM DCRM_CarsORData WITH (NOLOCK) WHERE InquiryId = @InquiryId AND ORPrice = 1
								IF @@ROWCOUNT = 0
									INSERT INTO DCRM_CarsORData(InquiryId, ORPrice) VALUES(@InquiryId, 1)
							END
						ELSE
							BEGIN
								UPDATE DCRM_CarsORData SET ORPrice = 0, UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy
								WHERE InquiryId = @InquiryId
							END
					END 
				
			END
		ELSE
			BEGIN
				IF @Type = 1
					UPDATE DCRM_CarsORData SET ORKm = 0, UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy
					WHERE InquiryId = @InquiryId
				
				IF @Type = 2
					UPDATE DCRM_CarsORData SET ORPrice = 0, UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy
					WHERE InquiryId = @InquiryId
			END
		
END

