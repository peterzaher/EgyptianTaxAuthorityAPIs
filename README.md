As of 2020/2021, Egyptian tax authority regulations has been set to enforce the process of electronic invoice issuance.

An electronic form of every invoice issued to a client, must be sent to local tax athorities.
Sending an electronic copy of your invoices requires you to sign these invoices with a digital signature, and attach that signature to a JSON/XML format
of your invoice.

Egyptian tax authority (ETA) exposed a REST APIs to taxpayer for invoice submission, and a number of other related operations.
For details, refer to ETA's SDK: https://sdk.invoicing.eta.gov.eg/

This solution addresses different operations required to prepare your invoices before sending.

Four main basic operations are required to prepare the invoices for submission.
These operation can be described as the following:

1. Creating a JSON version of the invoice that complies to the format of ETA, as described in the SDK.
2. Creating a canonical invoice format based on the JSON version.
3. Signing the canonical invoice format using an eSeal certificate.
4. Sending the JSON document along with the attached signature.



