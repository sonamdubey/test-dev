IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetInsuranceCustDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetInsuranceCustDetails]
GO

	
-- =============================================
-- Author:		Vinayak
-- Create date: 17 July 2015
-- Description:	Proc to get the details given cw lead id
-- =============================================
CREATE PROCEDURE [dbo].[GetInsuranceCustDetails]	
	@Id INT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT V.Make as makeName,V.MakeId as makeId,V.Model as modelName,V.ModelId as modelId, V.Version as versionName,V.VersionId as versionId,
	P.InsTypeNew as isNew,P.CarRegistrationDate as RegisterDate,P.PolicyExpiryDate as PolicyExpiry,P.NoClaimBonus as NCB,
	C.Name as cityName,S.Name as stateName
	,P.Mobile as mobile,P.Name as custName,P.Email as custEmail
	FROM INS_PremiumLeads P WITH (NOLOCK)
	INNER JOIN vwMMV V ON P.VersionId=V.VersionId
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID=P.CityId
	INNER JOIN States S WITH (NOLOCK) ON C.StateId=S.ID
	WHERE P.ID = @Id
END
