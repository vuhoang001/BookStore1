<template>
    <div>
        <h1>Publisher Data</h1>

        <!-- Single Publisher -->
        <div v-if="publisher">
            <p><strong>ID:</strong> {{ publisher.id }}</p>
            <p><strong>Name:</strong> {{ publisher.name }}</p>
        </div>
        <div v-else>
            <p>Loading publisher data...</p>
        </div>

        <hr>

        <!-- Publisher List -->
        <h2>Publisher List</h2>
        <div>
            <label>Page: <input v-model.number="page" type="number" min="0"></label>
            <label>Page Size: <input v-model.number="pageSize" type="number" min="1"></label>
            <button @click="fetchPublishers">Fetch</button>
        </div>
        <div v-if="publishers.length > 0">
            <ul>
                <li v-for="pub in publishers" :key="pub.id">
                    {{ pub.id }} - {{ pub.name }}
                </li>
            </ul>
            <p>Total: {{ totalElements }}</p>
        </div>
        <div v-else>
            <p>No publishers loaded.</p>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { Publisher } from '@/api/models/publisher.model';
import { publisherService } from '@/api/services/publishers/publisher.service';
import { PageResponse, PaginationParams } from '@/api/types/common';
import { onMounted, ref } from 'vue';

const publisher = ref<Publisher | null>(null);
const publishers = ref<Publisher[]>([]);
const totalElements = ref(0);
const page = ref(0);
const pageSize = ref(10);

onMounted(async () => {
    try {
        const id = '196a73b7-35d0-416e-0675-08de954cdd3b';
        publisher.value = await publisherService.getById(id);
    } catch (error) {
        console.error('Failed to fetch publisher data:', error);
    }
});

const fetchPublishers = async () => {
    try {
        const filter: PaginationParams = { pageIndex: page.value, pageSize: pageSize.value };
        const response: PageResponse<Publisher> = await publisherService.list(filter);
        publishers.value = response.data;
        totalElements.value = response.count;
    } catch (error) {
        console.error('Failed to fetch publishers:', error);
    }
};
</script>