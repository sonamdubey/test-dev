IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SaveWarrantyModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SaveWarrantyModel]
GO

	
-- =============================================
-- Author      : Vinay Kumar
-- Create date : 8 Aug 2013
-- Description : To insert and updated Absure Warranty Model Gold price Silver price .
-- Modified By : Yuga Hatolkar on March 23rd, 2015, Added Parameter @EligibleModelFor and fetched data respectively.
-- =============================================
CREATE PROCEDURE [dbo].[Absure_SaveWarrantyModel]
	@Id						 INT , 
	@TotalModelId			 VarChar(500),
	@GoldPrice    			 INT,
	@SilverPrice			 INT,	
	@UpdatedOn				 DATETIME,
	@UpdatedBy				 INT,
	@IsActive				 BIT,
	@IsWarrantyEligible		 BIT,
	@IsCertificationEligible BIT,	
	@EligibleModelFor		 TINYINT = 1,  -- 1: By default Warranty
	@RetVal					 TinyInt = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--For Insertion 
    IF (@Id = -1)
		BEGIN
					
			--deleting row from CRM_FLCMessage acording to selected id
		    -- DELETE FROM CRM_FLCMessage WHERE Id IN(SELECT * FROM list_to_tbl(@ID))
					
			DECLARE @idString VARCHAR(MAX)
			SET @idString = @TotalModelId +','
				
			WHILE CHARINDEX(',', @idString) > 0 
				BEGIN
					DECLARE @tmpstr VARCHAR(50)
						SET @tmpstr = SUBSTRING(@idString, 1, ( CHARINDEX(',',@idString ) - 1 ))

						SELECT AEM.Id FROM Absure_EligibleModels AS AEM WITH(NOLOCK) WHERE AEM.IsActive=@IsActive AND AEM.ModelId=CONVERT(INT,@tmpstr)
			            IF @@ROWCOUNT = 0	
							BEGIN					
								INSERT INTO Absure_EligibleModels(ModelId,SilverPrice,GoldPrice,IsActive,UpdatedBy,UpdatedOn, IsEligibleWarranty, IsEligibleCertification)
								VALUES(CONVERT(INT,@tmpstr),@SilverPrice,@GoldPrice,@IsActive,@UpdatedBy,@UpdatedOn, @IsWarrantyEligible, @IsCertificationEligible)
							END 
							ELSE
								BEGIN
									UPDATE Absure_EligibleModels SET SilverPrice=@SilverPrice,GoldPrice=@GoldPrice,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,
																	 IsEligibleWarranty=@IsWarrantyEligible,IsEligibleCertification=@IsCertificationEligible
									WHERE ModelId = CONVERT(INT,@tmpstr)
										  --AND (
												--( @EligibleModelFor = 1 AND ISNULL(IsEligibleWarranty,1) = 1) OR 
												--( @EligibleModelFor = 2 AND IsEligibleCertification = 1) OR 
												--( @EligibleModelFor = 3 AND (IsEligibleCertification = 1 AND IsEligibleWarranty = 1))
											 -- )
								END						 
					SET @idString = SUBSTRING(@idString, CHARINDEX(',', @idString) + 1, LEN(@idString))
					END

			SET @RetVal=  1		    
		END
		
	ELSE   -- For Updation 
		BEGIN
		    SELECT AEM.Id FROM Absure_EligibleModels AS AEM WITH(NOLOCK) WHERE AEM.IsActive=@IsActive AND AEM.ModelId=CONVERT(INT,@TotalModelId) AND AEM.Id<>@Id
		    IF @@ROWCOUNT = 0
				BEGIN
					UPDATE Absure_EligibleModels SET ModelId= CONVERT(INT,@TotalModelId),SilverPrice=@SilverPrice,GoldPrice=@GoldPrice,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,
													 IsEligibleWarranty=@IsWarrantyEligible,IsEligibleCertification=@IsCertificationEligible
					WHERE Id = @Id							  
					       
					SET @RetVal= 2
				END
				ELSE
					BEGIN
						SET @RetVal =3
					END
        END
END
