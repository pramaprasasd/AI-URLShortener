# Ambiguous Requirement Scenario

Requirement: "Provide analytics showing how many people clicked each link."

Ambiguities identified:
- What counts as a click?
- Are bots counted?
- What is a unique visitor?
- How long is analytics retained?
- Should analytics be real time?
- Should IP addresses be stored?
- What happens if analytics storage fails?

Decisions for this prototype:
- Every successful redirect attempts to create one click event.
- Bot filtering is out of scope.
- Unique visitors are not claimed by the MVP.
- Analytics is eventually consistent.
- Analytics failure does not fail the redirect.
- Raw IP is stored only as request metadata for the prototype and should be governed/retained appropriately in a production deployment.
