import axios from "axios";

export async function get_client_analytics(cli_id) {
    let harcodedObject = Promise.resolve({
        data: {
        attendedTrainings: 10,
        cancelledTrainings: 2,
        averageRating: 7.4,
        trainersWorkedWith: [
            {
            fullName: "Vukasin Markovic",
            contactEmail: "vmark@fitness.com",
            contactPhone: "+38160123456",
            trainingTypes: [
                { name: "yoga" },
                { name: "pilates "}
            ],
            averageRating: 10.0
            }
        ]
        }
    });

    return harcodedObject;
}
