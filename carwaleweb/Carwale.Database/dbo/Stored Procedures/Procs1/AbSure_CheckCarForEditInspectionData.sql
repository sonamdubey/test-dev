IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_CheckCarForEditInspectionData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_CheckCarForEditInspectionData]
GO

	
-- ===================================================================
-- Author      :  Yuga Hatolkar
-- Create date :  28 August 2015
-- Description :  Check Eligibility of the Car for Edit Inspection.
-- Modified By :  Nilima More, 11th Sept 2015.
-- Description :  Changed entire logic.
-- =====================================================================
CREATE PROCEDURE [dbo].[AbSure_CheckCarForEditInspectionData] @AbSure_CarDetailsId BIGINT
	, @IsEligible BIT = NULL OUTPUT
AS
BEGIN
	DECLARE @AbsureCarStatus INT = NULL
		, @RegistrationNumber VARCHAR(200) = NULL
		, @ActivationEndDate DATETIME = NULL
		, @SurveyDate DATETIME = NULL
		, @CertificateExpiryDays INT = NULL
		, @DealerId BIGINT = NULL
		, @PreviousCarId INT = NULL
		, @CertificateExpiryDaysTemp INT = NULL
		,@TC_RolesMasterId INT = NULL

	SELECT @DealerId = DealerId
	FROM AbSure_CarDetails WITH (NOLOCK)
	WHERE Id = @AbSure_CarDetailsId

	SELECT @TC_RolesMasterId = TC_RolesMasterId FROM TC_RolesMaster  WITH (NOLOCK)

	-----Quikr, CampBTL and Absure.in dealers are to be excluded------
	IF (
			@DealerId = 11894
			OR @DealerId = 11392
			OR @DealerId = 12150
			OR @TC_RolesMasterId = 14
			)
	BEGIN
		SET @IsEligible = 1
	END
	ELSE
	BEGIN
		--PRINT '1'
		SELECT @AbsureCarStatus = STATUS
			, @RegistrationNumber = RegNumber
			, @SurveyDate = SurveyDate
		FROM AbSure_CarDetails WITH (NOLOCK)
		WHERE Id = @AbSure_CarDetailsId

		IF @AbsureCarStatus IN (
				1
				, 2
				, 9
				)
		BEGIN
			----ENTER CAR CERTICICATE IS EXPIRED--
			SELECT @CertificateExpiryDays = DATEDIFF(DD, @SurveyDate, GETDATE())

			IF (@CertificateExpiryDays >= 30)
			BEGIN
				--PRINT '01'
				SET @IsEligible = 0
			END
			ELSE
			BEGIN
				SELECT Id
				FROM AbSure_CarDetails
				WHERE (
						RegNumber = @RegistrationNumber
						AND ID <> @AbSure_CarDetailsId
						AND DealerId NOT IN (
							3838
							, 4271
							, 11392
							, 11894
							, 12150
							)
						)
				ORDER BY EntryDate DESC

				IF @@ROWCOUNT > 0
				BEGIN
					---STATUS 8 AND CHECK FOR EXPIRED WARRANTY---
					SELECT AbSure_CarDetailsId
					FROM AbSure_ActivatedWarranty AA WITH (NOLOCK)
					LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
					WHERE AA.RegNumber = @RegistrationNumber
						AND ACD.STATUS = 8
						AND Id <> @AbSure_CarDetailsId

					IF @@ROWCOUNT > 0
					BEGIN
						--PRINT '4'
						SELECT TOP 1 @PreviousCarId = ACD.Id
						FROM AbSure_ActivatedWarranty AA WITH (NOLOCK)
						LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
						WHERE AA.RegNumber = @RegistrationNumber
							AND ACD.STATUS = 8
							AND Id <> @AbSure_CarDetailsId
						ORDER BY AA.EntryDate DESC

						SELECT @ActivationEndDate = WarrantyEndDate
						FROM AbSure_ActivatedWarranty WITH (NOLOCK)
						WHERE RegNumber = @RegistrationNumber
							AND AbSure_CarDetailsId = @PreviousCarId

						-------CARS WITH EXPIRED WARRENTY BUT UNEXPIRED CERTIFICATE WITH SAME REGISTRATION NUMBER
						SELECT @SurveyDate = SurveyDate
						FROM AbSure_CarDetails
						WHERE STATUS IN (
								1
								, 4
								, 9
								, 8
								)
							AND RegNumber = @RegistrationNumber
							AND Id NOT IN (
								@AbSure_CarDetailsId
								, @PreviousCarId
								)
						ORDER BY EntryDate DESC

						IF (@@ROWCOUNT > 0)
						BEGIN
							--PRINT 'YUGA'
							--PRINT @PreviousCarId
							SELECT @CertificateExpiryDays = DATEDIFF(DD, @SurveyDate, GETDATE())
						END

						IF (GETDATE() >= @ActivationEndDate)
						BEGIN
							--PRINT @CertificateExpiryDays
							IF (
									@CertificateExpiryDays >= 30
									OR @CertificateExpiryDays IS NULL
									OR @CertificateExpiryDays = 0
									)
							BEGIN
								--PRINT 'N1'
								SET @IsEligible = 1
							END
							ELSE
							BEGIN
								--PRINT 'N2'
								--PRINT @CertificateExpiryDays
								SET @IsEligible = 0
							END
						END
						ELSE
						BEGIN
							--PRINT '6'
							SET @IsEligible = 0
						END
					END
					ELSE
					BEGIN
						--PRINT '7'
						-------Car id with status = 4 and have Activated warranty-------
						SELECT AbSure_CarDetailsId
						FROM AbSure_ActivatedWarranty AA WITH (NOLOCK)
						LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
						WHERE AA.RegNumber = @RegistrationNumber
							AND ACD.STATUS = 4
							AND Id <> @AbSure_CarDetailsId
						ORDER BY AA.EntryDate DESC

						IF @@ROWCOUNT > 0
						BEGIN
							--PRINT '44'
							----STATUS 4 BUT ENTRY IN ACTIVATED WAARANTY TABLE---
							SELECT TOP 1 @PreviousCarId = ACD.Id
							FROM AbSure_ActivatedWarranty AA WITH (NOLOCK)
							LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON AA.AbSure_CarDetailsId = ACD.Id
							WHERE AA.RegNumber = @RegistrationNumber
								AND ACD.STATUS = 4
								AND Id <> @AbSure_CarDetailsId
							ORDER BY AA.EntryDate DESC

							SELECT @ActivationEndDate = WarrantyEndDate
							FROM AbSure_ActivatedWarranty WITH (NOLOCK)
							WHERE RegNumber = @RegistrationNumber
								AND AbSure_CarDetailsId = @PreviousCarId

							IF (GETDATE() >= @ActivationEndDate)
							BEGIN
								--PRINT '55'
								SET @IsEligible = 1
							END
							ELSE
							BEGIN
								--PRINT '66'
								SET @IsEligible = 0
							END
						END
						ELSE
						BEGIN
							SELECT Id
							FROM AbSure_CarDetails WITH (NOLOCK)
							WHERE RegNumber = @RegistrationNumber
								AND STATUS = 4
								AND Id <> @AbSure_CarDetailsId
							ORDER BY EntryDate DESC

							IF @@ROWCOUNT > 0
							BEGIN
								--PRINT '8'
								-----status = 4 with expired certificate-----
								SELECT TOP 1 @SurveyDate = SurveyDate
								FROM AbSure_CarDetails WITH (NOLOCK)
								WHERE RegNumber = @RegistrationNumber
									AND STATUS = 4
									AND Id <> @AbSure_CarDetailsId
								ORDER BY EntryDate DESC

								SELECT @CertificateExpiryDays = DATEDIFF(DD, @SurveyDate, GETDATE())

								---------------------------------------------------------------------------------------------------------------------------------	
								SELECT TOP 1 @SurveyDate = SurveyDate
								FROM AbSure_CarDetails WITH (NOLOCK)
								WHERE RegNumber = @RegistrationNumber
									AND STATUS IN (
										1
										, 9
										)
									AND Id <> @AbSure_CarDetailsId
								ORDER BY EntryDate DESC

								SELECT @CertificateExpiryDaysTemp = DATEDIFF(DD, @SurveyDate, GETDATE())

								------------------------------------------------------------------------------------------------------------------------------------
								IF (
										@CertificateExpiryDays >= 30
										OR @CertificateExpiryDaysTemp >= 30
										)
								BEGIN
									--PRINT '9'
									SET @IsEligible = 1
								END
								ELSE
								BEGIN
									--PRINT '10'
									SET @IsEligible = 0
								END
							END
							--ELSE
							--BEGIN
							--	--PRINT '11'
							--	SELECT Id
							--	FROM AbSure_CarDetails WITH (NOLOCK)
							--	WHERE RegNumber = @RegistrationNumber
							--		AND STATUS IN (
							--			1
							--			, 9
							--			)
							--		AND ID <> @AbSure_CarDetailsId

							--	IF @@ROWCOUNT > 0
							--	BEGIN
							--		--PRINT '12'
							--		--PRINT @AbSure_CarDetailsId
							--		-----status = 1 with expired certificate-----
							--		SELECT TOP 1 @SurveyDate = SurveyDate
							--		FROM AbSure_CarDetails WITH (NOLOCK)
							--		WHERE RegNumber = @RegistrationNumber
							--			AND STATUS = 1
							--			AND Id <> @AbSure_CarDetailsId
							--		ORDER BY EntryDate DESC

							--		--PRINT @SurveyDate
							--		SELECT @CertificateExpiryDays = DATEDIFF(DD, @SurveyDate, GETDATE())

							--		IF (@CertificateExpiryDays >= 30)
							--		BEGIN
							--			--PRINT '13'
							--			SET @IsEligible = 1
							--		END
							--		ELSE
							--		BEGIN
							--			--PRINT '14'
							--			SET @IsEligible = 0
							--		END
								--END
								ELSE
								BEGIN
									SELECT Id
									FROM AbSure_CarDetails WITH (NOLOCK)
									WHERE RegNumber = @RegistrationNumber
										AND STATUS = 2
										AND Id <> @AbSure_CarDetailsId

									IF @@ROWCOUNT > 0
									BEGIN
										--PRINT '15'
										SELECT TOP 1 @SurveyDate = SurveyDate
										FROM AbSure_CarDetails WITH (NOLOCK)
										WHERE RegNumber = @RegistrationNumber
											AND STATUS = 2
											AND Id <> @AbSure_CarDetailsId
										ORDER BY EntryDate DESC

										----status = 2 with expire certificate----
										SELECT @CertificateExpiryDays = DATEDIFF(DD, @SurveyDate, GETDATE())

										IF (@CertificateExpiryDays >= 30)
										BEGIN
											--PRINT '16'
											SET @IsEligible = 1
										END
										ELSE
										BEGIN
											--PRINT '17'
											SET @IsEligible = 0
										END
									END
									ELSE
									BEGIN
										--PRINT '18'
										SELECT Id
										FROM AbSure_CarDetails WITH (NOLOCK)
										WHERE RegNumber = @RegistrationNumber
											AND STATUS = 7
											AND Id <> @AbSure_CarDetailsId

										IF @@ROWCOUNT > 0
										BEGIN
											--PRINT '19'
											SELECT TOP 1 @SurveyDate = SurveyDate
											FROM AbSure_CarDetails WITH (NOLOCK)
											WHERE RegNumber = @RegistrationNumber
												AND STATUS = 7
												AND Id <> @AbSure_CarDetailsId
											ORDER BY EntryDate DESC

											SELECT @CertificateExpiryDays = DATEDIFF(DD, @SurveyDate, GETDATE())

											IF (@CertificateExpiryDays >= 30)
											BEGIN
												--PRINT '20'
												SET @IsEligible = 1
											END
											ELSE
											BEGIN
												--PRINT '21'
												SET @IsEligible = 0
											END
										END
										ELSE
										BEGIN
											--PRINT '21'
											SET @IsEligible = 0
										END
									END
								END
							END
						END
					END
				--END
				ELSE
				BEGIN
					--PRINT '22'
					SET @IsEligible = 1
				END
			END
		END
		ELSE
		BEGIN
			--PRINT '23'
			SET @IsEligible = 0 --STATUS =3,5,6
		END
	END
END