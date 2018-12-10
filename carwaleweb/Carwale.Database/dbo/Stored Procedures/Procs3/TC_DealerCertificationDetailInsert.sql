IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerCertificationDetailInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerCertificationDetailInsert]
GO

	

-- =============================================  
-- Author:  Manish  
-- Create date: 22-APR-2013 
-- Details: SP used to insert and update the record in TC_DealerCertificationDetail
-- =============================================  
CREATE PROCEDURE [dbo].[TC_DealerCertificationDetailInsert] @DealersID      [numeric](18, 0),    
                                                            @Description    VARCHAR(MAX),
                                                            @Advantages     VARCHAR(MAX),
															@Criteria       VARCHAR(MAX),
															@CoreBenefits   VARCHAR(MAX),
                                                            @CheckPoints    VARCHAR(MAX),
															@WarrantyServices  VARCHAR(MAX)

AS 
  BEGIN 
     
     IF EXISTS(SELECT DealersId FROM TC_DealerCertificationDetail WHERE DealersId=@DealersId)
      BEGIN
         UPDATE  TC_DealerCertificationDetail SET 	 Description=@Description,
												     Advantages=@Advantages,
												     Criteria=@Criteria,       
												     CoreBenefits=@CoreBenefits,
												     CheckPoints=@CheckPoints,    
												     WarrantyServices=@WarrantyServices
         WHERE  DealersId=@DealersId
      END
      ELSE  
           BEGIN       
              IF  EXISTS (SELECT Id FROM Dealers WHERE Id=@DealersId AND CertificationId<>-1 )
               BEGIN 
			  	 INSERT INTO TC_DealerCertificationDetail(DealersID,
														  Classified_CertifiedOrgId,
														  Description,
														  Advantages,
														  Criteria,
														  CoreBenefits,
														  CheckPoints,
														  WarrantyServices)
				 SELECT  D.ID,
						 D.CertificationId,
						 @Description,
						 @Advantages,
						 @Criteria,
						 @CoreBenefits,
						 @CheckPoints,
						 @WarrantyServices
				 FROM Dealers D WHERE ID=@DealersId													  
             END
           END      
       END
     

